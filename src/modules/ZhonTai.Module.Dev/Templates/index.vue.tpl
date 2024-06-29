@using ZhonTai.Module.Dev;
@using ZhonTai.DynamicApi.Enums;
@{
    var gen = Model as ZhonTai.Module.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var areaName = "" + gen.ApiAreaName;
    var entityName = "" + gen.EntityName;

    var areaNamePc = areaName.NamingPascalCase();
    var entityNamePc = entityName.NamingPascalCase();

    var areaNameKc = areaName.NamingKebabCase();// KebabCase(areaName);
    var entityNameKc = entityName.NamingKebabCase();// KebabCase(entityName);

    var areaNameCc = areaName.NamingCamelCase();// camelCase(areaName);
    var entityNameCc = entityName.NamingCamelCase();// camelCase(entityName);

    var at = "@";
    var apiName = entityName + "Api";

    var permissionArea = string.Concat(areaNameKc, ":", entityNameKc);

    var queryColumns = gen.Fields.Where(w => w.WhetherQuery);

    var defineUiComponentsImportPath = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","@/components/my-select-window/dictionary" },
        //{"my-role","@/components/my-select-window/role" },
        //{"my-user","@/components/my-select-window/user"},
        //{"my-position","@/components/my-select-window/position" }
        //{"my-upload","@/components/my-upload/index" }
    };
    var uiComponentsMethodName = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","onOpenDic" },
        //{"my-role","onOpenRole" },
        //{"my-user","onOpenUser" },
        //{"my-position","onOpenPosition" }
    };
    var editors = gen.Fields.Select(s => s.Editor).Distinct();
    var uiComponentsInfo = editors
        .Where(w => defineUiComponentsImportPath.Keys.Contains(w))
        .Select(s => new { ImportName = s.NamingPascalCase(), ImportPath = defineUiComponentsImportPath[s] });



    // 获取数据输入控件
    string editorName(ZhonTai.Module.Dev.Domain.CodeGen.CodeGenFieldEntity col, out string attrs, out string innerBody)
    {
        attrs = string.Empty;
        innerBody = string.Empty;
        var editorName = col.Editor;
        if (String.IsNullOrWhiteSpace(editorName)) editorName = "el-input";
        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
            editorName = "el-select";
            if( col.IsNullable)attrs += " clearable ";
            innerBody = string.Concat("<el-option v-for=", "\"item in dicts['", col.DictTypeCode, "']\" :key=\"item.code\" :value=\"item.code\" :label=\"item.name\" />");
        }
        else if (col.Editor == "el-select")
        {
            editorName = col.Editor;
            if (col.IsNullable) attrs += " clearable ";
            if(!String.IsNullOrWhiteSpace(col.DisplayColumn) || !String.IsNullOrWhiteSpace(col.ValueColumn))
            {
                var labels = (""+col.DisplayColumn).Split(',');
                var values = ("" + col.ValueColumn).Split(',');
                var cout = labels.Length;
                if (values.Length > cout) cout = values.Length;
                for(var i = 0; i < cout; i++)
                {
                    innerBody += string.Concat("<el-option value=\"" + (values.Length > i ? values[i] : "") + "\" label=\"" + (labels.Length > i ? labels[i] : "") + "\" />");
                }
            }
        }
        else if(defineUiComponentsImportPath.Keys.Any(a => a == col.Editor))
        {
            attrs = attrs + " class=\"input-with-select\" ";
            innerBody = "<el-button slot=\"append\" icon=\"el-icon-more\" @click=\"" + uiComponentsMethodName[col.Editor] + "('editForm','" + col.DictTypeCode + "','" + col.Title + "')\" />";
        }
        else if (col.Editor == "my-upload")
        {
            editorName = "my-upload";
        }
        else if (col.Editor == "my-input-textarea"){
            editorName= "el-input";
            attrs += " type=\"textarea\" ";
        }
        else if (col.Editor == "my-input-number"){
            editorName= "el-input-number";
        }
        else if (col.Editor == "my-bussiness-select"){
            editorName= "el-select";
            if (col.IsNullable) attrs += " clearable ";
            if(!String.IsNullOrWhiteSpace(col.IncludeEntity)){
                //业务下拉前缀
                var selectPrefix = col.IncludeEntity.Replace("Entity", "");
                innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"item.id\" :label=\"item.title\" />");
            }
        }

        return editorName;
    }

    var dictCodes = gen.Fields.Where(w => "dict" == w.EffectType).Select(s => s.DictTypeCode);// editors.Any(a => a == "my-select-dictionary");
    var hasDict = dictCodes.Any();
    //关联的模型
    var includeFieldEntitys = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.IncludeEntity)).Select(w=>w.IncludeEntity.Replace("Entity", "")).Distinct();
    var hasUpload=editors.Any(a=>a=="my-upload");
    //var hasRole = editors.Any(a => a == "my-role");
    //var hasUser = editors.Any(a => a == "my-user");
    //var hasPosition = editors.Any(a => a == "my-position");

    string jsBool(Boolean exp){
        return exp ? "true" : "false";
    }
}
@{ 
    string attributes, inner;
}
<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @(at)submit.stop.prevent>
            @foreach (var col in queryColumns.Where(w=>!w.IsIgnoreColumn()))
            {
                var editor = editorName(col, out attributes, out inner);
            @:<el-form-item class="search-box-item">
            @:  <@(editor) @if(!attributes.Contains("clearable"))@("clearable") @(attributes) v-model="state.filter.@(col.ColumnName.NamingCamelCase())" placeholder="@(col.Title)" @(at)keyup.enter="onQuery" >
            if(!string.IsNullOrWhiteSpace(inner)){
            @:    @(inner)
            }
            @:  </@(editor)>
            @:</el-form-item>
            }
            @if (queryColumns.Count() > 0)
            {
            @:<el-form-item>
            @:  <el-button type="primary" icon="ele-Search" @(at)click="onQuery">查询</el-button>
            @:</el-form-item>
            }
          </el-form>
        </el-col>
        <el-col :span="6" class="text-right">
          <el-space>
          @if (gen.GenAdd){
          @:<el-button type="primary" icon="ele-Plus" @(at)click="onAdd">新增</el-button>
            }
            @if(gen.GenBatchDelete || gen.GenBatchSoftDelete){
            @:<el-dropdown :placement="'bottom-end'">
            @:  <el-button type="warning">批量操作 <el-icon><ele-ArrowDown /></el-icon></el-button>
            @:  <template #dropdown>
            @:    <el-dropdown-menu>
                if(gen.GenBatchSoftDelete){
                  @:  <el-dropdown-item v-auth="perms.batSoftDelete" :disabled="state.sels.length==0" @(at)click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-dropdown-item>
                  }
                  if(gen.GenBatchDelete){
                  @:  <el-dropdown-item v-auth="perms.batDelete"  :disabled="state.sels.length==0" @(at)click="onBatchDelete" icon="ele-Delete">批量删除</el-dropdown-item>
                  }
            @:    </el-dropdown-menu>
            @:  </template>
            @:</el-dropdown>
             }
          </el-space>
        </el-col>
      </el-row>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.@(entityNameCc)ListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @(at)selection-change="selsChange">
        
        @if(gen.GenDelete||gen.GenSoftDelete){
          @:<el-table-column type="selection" width="50" />
          }
              @foreach (var col in gen.Fields.Where(w => w.WhetherTable && !w.IsIgnoreColumn()))
              {
                  if(col.IsIncludeColumn()&&!string.IsNullOrWhiteSpace(col.IncludeEntityKey)){
                      if(col.IncludeMode==0){
          @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Text" label="@(col.Title)" show-overflow-tooltip width />
                      }else if(col.IncludeMode==1){
                          
          @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Texts" label="@(col.Title)" show-overflow-tooltip width >
          @:  <template #default="{ row }">
          @:    {{ row.@(col.ColumnName.NamingCamelCase())_Texts ? row.@(col.ColumnName.NamingCamelCase())_Texts.join(',') : '' }}
          @:  </template>
          @:</el-table-column>
                      }
                  }else if(col.Editor=="my-upload"){
          @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())" label="@(col.Title)" show-overflow-tooltip width >
          @:  <template #default="{ row }">
          @:   <div class="my-flex">
          @:     <el-image :src="row.@(col.ColumnName.NamingCamelCase())" :preview-src-list="preview@(col.ColumnName)list"
          @:       :initial-index="get@(col.ColumnName)InitialIndex(row.@(col.ColumnName.NamingCamelCase()))" :lazy="true" :hide-on-click-modal="true" fit="scale-down"
          @:       preview-teleported style="width: 80px; height: 80px" />
          @:     <div class="ml10 my-flex-fill my-flex-y-center">
          @:     </div>
          @:   </div>
          @: </template>
          @:</el-table-column>
                  }else{
          @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())@if(!string.IsNullOrWhiteSpace(col.DictTypeCode))@("DictName")" label="@(col.Title)" show-overflow-tooltip width />
                  }
              }
          <el-table-column v-auths="[perms.update,perms.softDelete,perms.delete]" label="操作" :width="actionColWidth" fixed="right">
            <template #default="{ row }">
              <el-button v-auth="perms.update" icon="ele-EditPen" size="small" text type="primary" @(at)click="onEdit(row)">编辑</el-button>
              @if(gen.GenDelete&&gen.GenSoftDelete){
              @:<el-dropdown v-if="authAll([perms.delete,perms.softDelete])" style="margin:5px 0 0 5px;">
              @:  <span><el-icon class="el-icon--right"><component :is="'ele-ArrowDown'" /></el-icon></span>
              @:  <template #dropdown>
              @:    <el-dropdown-menu>
              @:      <el-dropdown-item v-if="auth(perms.delete)" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-dropdown-item>
              @:      <el-dropdown-item v-if="auth(perms.softDelete)" @(at)click="onSoftDelete(row)" icon="ele-DeleteFilled">软删除</el-dropdown-item>
              @:    </el-dropdown-menu>
              @:  </template>            
              @:</el-dropdown>
              @:<span v-else style="margin-left:5px;height:inherit">
              @:  <el-button text type="warning" v-if="auth(perms.softDelete)" style="height:inherit" @(at)click="onDelete(row)" icon="ele-DeleteFilled">软删除</el-button>
              @:  <el-button text type="danger" v-if="auth(perms.delete)" style="height:inherit" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-button>
              @:</span>
              }
              @if(gen.GenSoftDelete&&!gen.GenDelete){
              @:<el-button text type="warning" v-if="perms.softDelete" @(at)click="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-button>
              }
              @if(gen.GenDelete&&!gen.GenSoftDelete){
              @:<el-button text type="danger" v-auth="perms.delete" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-button>
              }
            </template>
          </el-table-column>
      </el-table>

      <div class="my-flex my-flex-end" style="margin-top: 20px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          small
          background
          @(at)size-change="onSizeChange"
          @(at)current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <@(entityNameKc)-form ref="@(entityNameCc)FormRef" :title="state.@(entityNameCc)FormTitle"></@(entityNameKc)-form>
  </div>
</template>

<script lang="ts" setup name="@(areaNameCc)/@(entityNameKc)">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInput@(entityNamePc)GetPageInput, @(entityNamePc)GetPageInput, @(entityNamePc)GetPageOutput, @(entityNamePc)GetOutput, @(entityNamePc)AddInput, @(entityNamePc)UpdateInput,
@if(gen.GenGetList){
@:  @(entityNamePc)GetListInput, @(entityNamePc)GetListOutput,
}
@{
    if (includeFieldEntitys.Any())
    {
        foreach(var incField in includeFieldEntitys)
        {
@:  @(incField)GetListOutput,
@:  @(incField)GetOutput,                    
        }
    }
}
} from '/@(at)/api/@(areaNameKc)/data-contracts'
@if(gen.Fields.Any(s=>s.Editor=="my-upload")){
@:import {  FileGetPageOutput } from '/@(at)/api/admin/data-contracts'
}
import { @(apiName) } from '/@(at)/api/@(areaNameKc)/@(entityNamePc)'
@if (includeFieldEntitys.Any())
{
    foreach(var incField in includeFieldEntitys)
    {
@:import { @(incField)Api } from '/@(at)/api/@(areaNameKc)/@(incField)'
    }
}

import eventBus from '/@(at)/utils/mitt'
import { auth, auths, authAll } from '/@(at)/utils/authFunction'

// 引入组件
const @(entityNamePc)Form = defineAsyncComponent(() => import('./components/@(entityNameKc)-form.vue'))

const { proxy } = getCurrentInstance() as any

const @(entityNameCc)FormRef = ref()

//权限配置
const perms = {
  add:'api:@(permissionArea):add',
  update:'api:@(permissionArea):update',
  delete:'api:@(permissionArea):delete',
  batDelete:'api:@(permissionArea):batch-delete',
  softDelete:'api:@(permissionArea):soft-delete',
  batSoftDelete:'api:@(permissionArea):batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  @(entityNameCc)FormTitle: '',
  total: 0,
  sels: [] as Array<@(entityNamePc)GetPageOutput>,
  filter: {
@foreach(var f in queryColumns.Where(w=>!w.IsIgnoreColumn())){
@:    @(f.ColumnName.NamingCamelCase()): null,
}
  } as @(entityNamePc)GetPageInput @if(gen.GenGetList)@("| " + entityNamePc+"GetListInput"),
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInput@(entityNamePc)GetPageInput,
  @(entityNameCc)ListData: [] as Array<@(entityNamePc)GetListOutput>,
  @foreach(var incField in includeFieldEntitys){
@:  select@(incField)ListData: [] as @(incField)GetListOutput[],
}
@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  file@(col.ColumnName)ListData: [] as Array<FileGetPageOutput>,
}
})

onMounted(() => {

@foreach(var incField in includeFieldEntitys){
@:  get@(incField)List();
}
  onQuery()
  eventBus.off('refresh@(entityNamePc)')
  eventBus.on('refresh@(entityNamePc)', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refresh@(entityNamePc)')
})

@foreach(var incField in includeFieldEntitys){
@:const get@(incField)List = async () => {
@:  const res = await new @(incField)Api().getList({}).catch(() => {
@:    state.select@(incField)ListData = []
@:  })
@:  state.select@(incField)ListData = res?.data || []
@:}
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new @(apiName)().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.@(entityNameCc)ListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false

  @foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  state.file@(col.ColumnName)ListData = res?.data?.list?.map(s => {
@:    return { linkUrl: s.@(col.ColumnName.NamingCamelCase()) }
@:  }) ?? []
}


}

const onAdd = () => {
  state.@(entityNameCc)FormTitle = '新增@(gen.BusName)'
  @(entityNameCc)FormRef.value.open()
}

const onEdit = (row: @(entityNamePc)GetOutput) => {
  state.@(entityNameCc)FormTitle = '编辑@(gen.BusName)'
  @(entityNameCc)FormRef.value.open(row)
}

const onDelete = (row: @(entityNamePc)GetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new @(apiName)().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onSizeChange = (val: number) => {
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}
const selsChange = (vals: @(entityNamePc)GetPageOutput[]) => {
  state.sels = vals
}

@if(gen.GenBatchDelete){
@:const onBatchDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () =>{
@:    const rst = await new @(apiName)().batchDelete(state.sels?.map(item=>item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

@if(gen.GenSoftDelete){
@:const onSoftDelete = async (row: @(entityNamePc)GetOutput) => {
@:  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
@:    const rst = await new @(apiName)().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

@if(gen.GenBatchSoftDelete){
@:const onBatchSoftDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
@:    const rst = await new @(apiName)().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}
@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:const preview@(col.ColumnName)list = computed(() => {
@:  let imgList = [] as string[]
@:  state.file@(col.ColumnName)ListData.forEach((a) => {
@:    if (a.linkUrl) {
@:      imgList.push(a.linkUrl as string)
@:    }
@:  })
@:  return imgList
@:})
@:const get@(col.ColumnName)InitialIndex = (imgUrl: string) => {
@:  return preview@(col.ColumnName)list.value.indexOf(imgUrl)
@:}
}
</script>
