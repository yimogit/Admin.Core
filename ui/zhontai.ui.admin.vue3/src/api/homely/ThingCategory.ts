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
  PageInputThingCategoryGetPageInput,
  ResultOutputBoolean,
  ResultOutputIEnumerableThingCategoryGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputThingCategoryGetPageOutput,
  ResultOutputThingCategoryGetOutput,
  ThingCategoryAddInput,
  ThingCategoryGetListInput,
  ThingCategoryUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ThingCategoryApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags thing-category
   * @name Add
   * @summary 新增
   * @request POST:/api/homely/thing-category/add
   * @secure
   */
  add = (data: ThingCategoryAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/homely/thing-category/add`,
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
   * @tags thing-category
   * @name Get
   * @summary 查询
   * @request GET:/api/homely/thing-category/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputThingCategoryGetOutput, any>({
      path: `/api/homely/thing-category/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-category
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/homely/thing-category/get-page
   * @secure
   */
  getPage = (data: PageInputThingCategoryGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputThingCategoryGetPageOutput, any>({
      path: `/api/homely/thing-category/get-page`,
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
   * @tags thing-category
   * @name Update
   * @summary 更新
   * @request PUT:/api/homely/thing-category/update
   * @secure
   */
  update = (data: ThingCategoryUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/homely/thing-category/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-category
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/homely/thing-category/delete
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
      path: `/api/homely/thing-category/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-category
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/homely/thing-category/get-list
   * @secure
   */
  getList = (data: ThingCategoryGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableThingCategoryGetListOutput, any>({
      path: `/api/homely/thing-category/get-list`,
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
   * @tags thing-category
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/homely/thing-category/soft-delete
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
      path: `/api/homely/thing-category/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-category
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/homely/thing-category/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/homely/thing-category/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
