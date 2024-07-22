﻿using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Captcha;
using ZhonTai.Admin.Core.Dto;
using static Lazy.SlideCaptcha.Core.ValidateResult;
using System.Threading.Tasks;
using System;
using ZhonTai.Common.Helpers;
using DotNetCore.CAP;
using ZhonTai.Admin.Services.Captcha.Dto;
using ZhonTai.Admin.Services.Msg.Events;

namespace ZhonTai.Admin.Services.Cache;

/// <summary>
/// 验证码服务
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CaptchaService : BaseService, IDynamicApi
{
    private readonly ICaptcha _captcha;
    private readonly ISlideCaptcha _slideCaptcha;
    private readonly ICapPublisher _capPublisher;

    public CaptchaService(ICaptcha captcha, ISlideCaptcha slideCaptcha, ICapPublisher capPublisher)
    {
        _captcha = captcha;
        _slideCaptcha = slideCaptcha;
        _capPublisher = capPublisher;
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public CaptchaData Generate(string captchaId = null)
    {
        return _captcha.Generate(captchaId);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="captchaId">验证码Id</param>
    /// <param name="track">滑动轨迹</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public ValidateResult CheckAsync([FromQuery] string captchaId, SlideTrack track)
    {
        if (captchaId.IsNull() || track == null)
        {
            throw ResultOutput.Exception("请完成安全验证");
        }

        return _slideCaptcha.Validate(captchaId, track, false);
    }

    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<string> SendSmsCodeAsync(SendSmsCodeInput input)
    {
        if (input.Mobile.IsNull())
        {
            throw ResultOutput.Exception("请输入手机号");
        }

        if (input.CaptchaId.IsNull() || input.Track == null)
        {
            throw ResultOutput.Exception("请完成安全验证");
        }

        var validateResult = _captcha.Validate(input.CaptchaId, input.Track);
        if (validateResult.Result != ValidateResultType.Success)
        {
            throw ResultOutput.Exception($"安全{validateResult.Message}");
        }

        var codeId = input.CodeId.IsNull() ? Guid.NewGuid().ToString() : input.CodeId;
        var code = StringHelper.GenerateRandomNumber();
        await Cache.SetAsync(CacheKeys.GetSmsCodeKey(input.Mobile, codeId), code, TimeSpan.FromMinutes(5));

        //发送短信验证码
        await _capPublisher.PublishAsync(SubscribeNames.SmsSendCode, new
        {
            input.Mobile,
            Text = code
        });

        return codeId;
    }

    /// <summary>
    /// 发送邮件验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<string> SendEmailCodeAsync(SendEmailCodeInput input)
    {
        if (input.Email.IsNull())
        {
            throw ResultOutput.Exception("请输入邮件地址");
        }

        if (input.CaptchaId.IsNull() || input.Track == null)
        {
            throw ResultOutput.Exception("请完成安全验证");
        }

        var validateResult = _captcha.Validate(input.CaptchaId, input.Track);
        if (validateResult.Result != ValidateResultType.Success)
        {
            throw ResultOutput.Exception($"安全{validateResult.Message}");
        }

        var codeId = input.CodeId.IsNull() ? Guid.NewGuid().ToString() : input.CodeId;
        var code = StringHelper.GenerateRandomNumber();
        await Cache.SetAsync(CacheKeys.GetEmailCodeKey(input.Email, codeId), code, TimeSpan.FromMinutes(5));

        //发送邮箱验证码
        await _capPublisher.PublishAsync(SubscribeNames.EmailSendCode, new EmailSendCodeEvent
        {
            ToEmail = new EmailSendCodeEvent.Models.EmailModel
            {
                Address = input.Email,
            },
            Code = code,
        });

        return codeId;
    }
}