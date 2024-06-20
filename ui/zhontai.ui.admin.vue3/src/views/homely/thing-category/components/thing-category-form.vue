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
        <el-form-item label="排序" prop="sort" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.form.sort" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="分类名称" prop="name" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.form.name" placeholder="" >
          </el-input>
        </el-form-item>
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

<script lang="ts" setup name="homely/thing-category/form">
import { reactive, toRefs, getCurrentInstance, ref } from 'vue'
import { ThingCategoryAddInput, ThingCategoryUpdateInput,
  ThingCategoryGetListInput, ThingCategoryGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingCategoryApi } from '/@/api/homely/ThingCategory'
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
  form: {} as ThingCategoryAddInput | ThingCategoryUpdateInput,
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {

  if (row.id > 0) {
    const res = await new ThingCategoryApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as ThingCategoryUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const defaultToAdd = (): ThingCategoryAddInput => {
  return {
    sort: null,
    name: null,
    tenantId: null,
  } as ThingCategoryAddInput
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
      res = await new ThingCategoryApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new ThingCategoryApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshThingCategory')
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
