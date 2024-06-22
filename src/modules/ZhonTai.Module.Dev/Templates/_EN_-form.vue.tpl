﻿@using ZhonTai.Module.Dev;
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
    string editorName(ZhonTai.Module.Dev.Domain.CodeGen.CodeGenFieldEntity col, out string attrs, out string innerBody,out string subfix)
    {
        attrs = string.Empty;
        subfix = string.Empty;
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
            attrs += " v-if='state.showDialog' ";
        }
        else if (col.Editor == "my-input-textarea"){
            editorName= "el-input";
            attrs += " type=\"textarea\" ";
        }
        else if (col.Editor == "my-input-number"){
            editorName= "el-input";
            attrs += " type=\"number\" ";
        }
        else if (col.Editor == "my-bussiness-select"){
            editorName= "el-select";
            if (col.IsNullable) attrs += " clearable ";
            if (col.IncludeMode == 1){
                attrs += " multiple ";
                subfix="_Values";
            }
            if(!String.IsNullOrWhiteSpace(col.IncludeEntity)){
                //业务下拉前缀
                var selectPrefix = col.IncludeEntity.Replace("Entity", "");
                var selectTitle="name";
                if(!String.IsNullOrWhiteSpace(col.IncludeEntityKey))
                    selectTitle=col.IncludeEntityKey.NamingCamelCase();
                if (col.IncludeMode == 1){
                    //一对多,转换模型 xxxIds_Values
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"String(item.id)\" :label=\"item.",selectTitle,"\" />");
                }else{
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"item.id\" :label=\"item.",selectTitle,"\" />");
                }
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
    string attributes, inner, subfix;
}
<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="769px"
    >
      <el-form ref="formRef" :model="form" size="default" label-width="80px">
      @foreach(var col in gen.Fields.Where(w=>!w.IsIgnoreColumn() && ( w.WhetherAdd || w.WhetherUpdate )))
      {
        var editor = editorName(col, out attributes, out inner,out subfix);
        @:<el-form-item label="@(col.Title)" prop="@(col.ColumnName.NamingCamelCase())@(subfix)" v-show="editItemIsShow(@jsBool(col.WhetherAdd), @jsBool(col.WhetherUpdate))">
        @:  <@(editor) @(attributes) v-model="state.form.@(col.ColumnName.NamingCamelCase())@(subfix)" placeholder="@(col.Comment)" >
        if(!string.IsNullOrWhiteSpace(inner)){
        @:    @(inner)
        }
        @:  </@(editor)>
        @:</el-form-item>
      }
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @(at)click="onCancel" size="default">取 消</el-button>
          <el-button type="primary" @(at)click="onSure" size="default" :loading="state.sureLoading">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="@(areaNameCc)/@(entityNameKc)/form">
import { reactive, toRefs, getCurrentInstance, ref, defineAsyncComponent} from 'vue'
import { @(entityNamePc)AddInput, @(entityNamePc)UpdateInput,
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
import { @(apiName) } from '/@(at)/api/@(areaNameKc)/@(entityNamePc)'
@if (includeFieldEntitys.Any())
{
    foreach(var incField in includeFieldEntitys)
    {
@:import { @(incField)Api } from '/@(at)/api/@(areaNameKc)/@(incField)'
    }
}
import { auth, auths, authAll } from '/@(at)/utils/authFunction'

    @if (hasDict)
    {
@:import dictTreeApi from '@(at)/api/admin/dictionary-tree'        
    }
    @foreach (var comp in uiComponentsInfo)
    {
@:import @(comp.ImportName) from '@(comp.ImportPath)'
    }
    @if (hasUpload)
    {
 @:const MyUpload = defineAsyncComponent(() => import('/@(at)/components/my-upload/index.vue'))      
    }

import eventBus from '/@(at)/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as @(entityNamePc)AddInput | @(entityNamePc)UpdateInput,
@foreach(var incField in includeFieldEntitys){
@:  select@(incField)ListData: [] as @(incField)GetListOutput[],
}

})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
    
@foreach(var incField in includeFieldEntitys){
@:  get@(incField)List();
}
  if (row.id > 0) {
    const res = await new @(apiName)().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as @(entityNamePc)UpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

@foreach(var incField in includeFieldEntitys){
@:const get@(incField)List = async () => {
@:  const res = await new @(incField)Api().getList({}).catch(() => {
@:    state.select@(incField)ListData = []
@:  })
@:  state.select@(incField)ListData = res?.data || []
@:}
}

const defaultToAdd = (): @(entityNamePc)AddInput => {
  return {
@foreach(var col in gen.Fields.Where(w=>!w.IsIgnoreColumn())){
@:    @(col.ColumnName.NamingCamelCase()): @(col.GetDefaultValueStringScript()),
}
  } as @(entityNamePc)AddInput
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new @(apiName)().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new @(apiName)().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refresh@(entityNamePc)')
      state.showDialog = false
    }
  })
}

const editItemIsShow = (add: Boolean, edit: Boolean): Boolean => {
    if(add && edit)return true;
    let isEdit=state.form.id != undefined && state.form.id > 0
    if(add && !isEdit)return true;
    if(edit && isEdit)return true;
    return false;
}


defineExpose({
  open,
})
</script>