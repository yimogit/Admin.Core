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

import { AxiosResponse } from 'axios'
import {
  PageInputThingGetPageInput,
  ResultOutputBoolean,
  ResultOutputIEnumerableThingGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputThingGetPageOutput,
  ResultOutputThingGetOutput,
  ThingAddInput,
  ThingGetListInput,
  ThingUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ThingApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags thing
   * @name Get
   * @summary 查询
   * @request GET:/api/homely/thing/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputThingGetOutput, any>({
      path: `/api/homely/thing/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/homely/thing/get-list
   * @secure
   */
  getList = (data: ThingGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableThingGetListOutput, any>({
      path: `/api/homely/thing/get-list`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/homely/thing/get-page
   * @secure
   */
  getPage = (data: PageInputThingGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputThingGetPageOutput, any>({
      path: `/api/homely/thing/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name Add
   * @summary 新增
   * @request POST:/api/homely/thing/add
   * @secure
   */
  add = (data: ThingAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/homely/thing/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name Update
   * @summary 更新
   * @request PUT:/api/homely/thing/update
   * @secure
   */
  update = (data: ThingUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/homely/thing/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/homely/thing/delete
   * @secure
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/homely/thing/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/homely/thing/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/homely/thing/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/homely/thing/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/homely/thing/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
