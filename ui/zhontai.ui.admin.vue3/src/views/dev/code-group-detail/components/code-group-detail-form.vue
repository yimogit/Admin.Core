﻿<template>
  <div>
    <el-dialog v-model="state.showDialog" :title="title" draggable destroy-on-close :close-on-click-modal="false"
      :close-on-press-escape="false" class="my-dialog-form">
      <el-form ref="formRef" :model="form" size="default" label-width="auto">
        <el-row :gutter="20">
        <el-col :span="12">
           <el-form-item label="模板名称" prop="name" v-show="editItemIsShow(true, true)">
             <el-input  v-model="state.form.name" placeholder="" >
             </el-input>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="模板分组" prop="groupId" v-show="editItemIsShow(true, true)">
             <el-select  v-model="state.form.groupId" placeholder="" >
               <el-option v-for="item in state.selectCodeGroupListData" :key="item.id" :value="item.id" :label="item.name" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="生成路径" prop="path" v-show="editItemIsShow(true, true)">
             <el-input  v-model="state.form.path" placeholder="" >
             </el-input>
           </el-form-item>
        </el-col>
        <el-col :span="12">
           <el-form-item label="模板分组" prop="groupIds_Values" v-show="editItemIsShow(true, true)">
             <el-select  clearable  multiple  v-model="state.form.groupIds_Values" placeholder="" >
               <el-option v-for="item in state.selectCodeGroupListData" :key="item.id" :value="String(item.id)" :label="item.name" />
             </el-select>
           </el-form-item>
        </el-col>
        <el-col :span="24">
           <el-form-item label="模板内容" prop="content" v-show="editItemIsShow(true, true)">
             <el-input  type="textarea"  v-model="state.form.content" placeholder="" >
             </el-input>
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

<script lang="ts" setup name="dev/code-group-detail/form">
import { reactive, toRefs, getCurrentInstance, ref, defineAsyncComponent} from 'vue'
import { CodeGroupDetailAddInput, CodeGroupDetailUpdateInput,
  CodeGroupDetailGetListInput, CodeGroupDetailGetListOutput,
  CodeGroupGetListOutput,
  CodeGroupGetOutput,
} from '/@/api/dev/data-contracts'
import { CodeGroupDetailApi } from '/@/api/dev/CodeGroupDetail'
import { CodeGroupApi } from '/@/api/dev/CodeGroup'
import { auth, auths, authAll } from '/@/utils/authFunction'


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
  form: {} as CodeGroupDetailAddInput | CodeGroupDetailUpdateInput,
  selectCodeGroupListData: [] as CodeGroupGetListOutput[],
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
    
  getCodeGroupList();


  if (row.id > 0) {
    const res = await new CodeGroupDetailApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as CodeGroupDetailUpdateInput
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


const defaultToAdd = (): CodeGroupDetailAddInput => {
  return {
    name: "",
    groupId: 0,
    path: null,
    groupIds: null,
    content: "",
  } as CodeGroupDetailAddInput
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
      res = await new CodeGroupDetailApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new CodeGroupDetailApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshCodeGroupDetail')
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
