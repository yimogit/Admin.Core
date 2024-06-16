/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { ResultOutputIEnumerableDictionaryTreeOutput } from './data-contracts'
import { HttpClient, RequestParams } from './http-client'

export class DictionaryTreeApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dictionary-tree
   * @name Get
   * @summary 获取字典树
   * @request GET:/api/dev/dictionary-tree/get
   * @secure
   */
  get = (
    query?: {
      /** 类型编号列表 */
      codes?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputIEnumerableDictionaryTreeOutput, any>({
      path: `/api/dev/dictionary-tree/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
}
