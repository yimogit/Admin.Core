<template>
  <div>
    <el-dialog v-model="state.showDialog" :title="title" draggable destroy-on-close :close-on-click-modal="false"
      :close-on-press-escape="false" class="my-dialog-form">
      <el-form ref="formRef" :model="form" size="default" label-width="auto">
        <el-row :gutter="20">
        <el-col :span="12">
           <el-form-item label="文本框" prop="inputText" v-show="editItemIsShow(true, true)">
             <el-input  v-model="state.form.inputText" placeholder="请输入" >
             </el-input>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="数字框" prop="inputNumber" v-show="editItemIsShow(true, true)">
             <el-input-number  v-model="state.form.inputNumber" placeholder="" >
             </el-input-number>
           </el-form-item>
        </el-col>
        <el-col :span="24">
           <el-form-item label="文本域" prop="inputTextArea" v-show="editItemIsShow(true, true)">
             <el-input  type="textarea"  v-model="state.form.inputTextArea" placeholder="" >
             </el-input>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="日期" prop="inputDate" v-show="editItemIsShow(true, true)">
             <el-date-picker  value-format="YYYY-MM-DD" clearable v-model="state.form.inputDate" placeholder="" >
             </el-date-picker>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="开关" prop="inputSwitch" v-show="editItemIsShow(true, true)">
             <el-switch  v-model="state.form.inputSwitch" placeholder="" >
             </el-switch>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="下拉框" prop="inputSelectCustom" v-show="editItemIsShow(true, true)">
             <el-select  v-model="state.form.inputSelectCustom" placeholder="" >
               <el-option value="11" label="AA" /><el-option value="22" label="BB" /><el-option value="33" label="CC" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="复选框" prop="inputCheckbox" v-show="editItemIsShow(true, true)">
             <el-checkbox  v-model="state.form.inputCheckbox" placeholder="" >
             </el-checkbox>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="字典" prop="inputSelectDict" v-show="editItemIsShow(true, true)">
             <el-select  v-model="state.form.inputSelectDict" placeholder="" >
               <el-option v-for="item in state.dicts['sex']" :key="item.value" :value="item.value" :label="item.name" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="模块业务单选" prop="inputBussinessSingle" v-show="editItemIsShow(true, true)">
             <el-select  clearable  v-model="state.form.inputBussinessSingle" placeholder="" >
               <el-option v-for="item in state.selectCodeGroupListData" :key="item.id" :value="item.id" :label="item.name" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="模块业务多选" prop="inputBussinessMultiple_Values" v-show="editItemIsShow(true, true)">
             <el-select  clearable  multiple  v-model="state.form.inputBussinessMultiple_Values" placeholder="" >
               <el-option v-for="item in state.selectCodeGroupListData" :key="item.id" :value="String(item.id)" :label="item.name" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="24">
           <el-form-item label="图片上传" prop="inputImage" v-show="editItemIsShow(true, true)">
             <my-upload  v-if='state.showDialog'  v-model="state.form.inputImage" placeholder="" >
             </my-upload>
           </el-form-item>
        </el-col>
        <el-col :span="24">
           <el-form-item label="编辑器" prop="inputEditor" v-show="editItemIsShow(true, true)">
             <my-editor  v-if='state.showDialog'  v-model="state.form.inputEditor" placeholder="请输入内容" >
             </my-editor>
           </el-form-item>
        </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">取 消</el-button>
          <el-button type="primary" @click="onSure" size="default" :loading="state.sureLoading">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="dev/code-group-demo/form">
import { reactive, toRefs, getCurrentInstance, ref, defineAsyncComponent} from 'vue'
import { CodeGroupDemoAddInput, CodeGroupDemoUpdateInput,
  CodeGroupDemoGetListInput, CodeGroupDemoGetListOutput,
  CodeGroupGetListOutput,
  CodeGroupGetOutput,
} from '/@/api/dev/data-contracts'
import { CodeGroupDemoApi } from '/@/api/dev/CodeGroupDemo'
import { CodeGroupApi } from '/@/api/dev/CodeGroup'
import { auth, auths, authAll } from '/@/utils/authFunction'

import { DictApi } from '/@/api/admin/Dict'
 const MyUpload = defineAsyncComponent(() => import('/@/components/my-upload/index.vue'))      
 const MyEditor = defineAsyncComponent(() => import('/@/components/my-editor/index.vue'))      

import eventBus from '/@/utils/mitt'

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
  form: {} as CodeGroupDemoAddInput | CodeGroupDemoUpdateInput,
  selectCodeGroupListData: [] as CodeGroupGetListOutput[],
  //字典相关
  dicts:{
    "sex":[],   
  }
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
    
  getCodeGroupList();

  getDictsTree()      

  if (row.id > 0) {
    const res = await new CodeGroupDemoApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as CodeGroupDemoUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const getCodeGroupList = async () => {
  const res = await new CodeGroupApi().getList({}).catch(() => {
    state.selectCodeGroupListData = []
  })
  state.selectCodeGroupListData = res?.data || []
}

//获取需要使用的字典树
const getDictsTree = async () => {
  let res = await new DictApi().getList(['sex'])
  if(!res?.success)return;
    state.dicts = res.data
}

const defaultToAdd = (): CodeGroupDemoAddInput => {
  return {
    inputText: "",
    inputNumber: null,
    inputTextArea: null,
    inputDate: null,
    inputSwitch: null,
    inputSelectCustom: "",
    inputCheckbox: false,
    inputSelectDict: "",
    inputBussinessSingle: null,
    inputBussinessMultiple: null,
    inputImage: null,
    inputEditor: null,
  } as CodeGroupDemoAddInput
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
      res = await new CodeGroupDemoApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new CodeGroupDemoApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshCodeGroupDemo')
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
