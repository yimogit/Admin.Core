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
        <el-form-item label="物品名称" prop="name" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.form.name" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="图片" prop="imageUrl" v-show="editItemIsShow(true, true)">
          <my-upload  v-if='state.showDialog'  v-model="state.form.imageUrl" placeholder="" >
          </my-upload>
        </el-form-item>
        <el-form-item label="有效期" prop="availableDate" v-show="editItemIsShow(true, true)">
          <el-date-picker  v-model="state.form.availableDate" placeholder="" >
          </el-date-picker>
        </el-form-item>
        <el-form-item label="备注" prop="remark" v-show="editItemIsShow(true, true)">
          <el-input  type="textarea"  v-model="state.form.remark" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="排序" prop="sort" v-show="editItemIsShow(true, true)">
          <el-input-number  v-model="state.form.sort" placeholder="" >
          </el-input-number>
        </el-form-item>
        <el-form-item label="分类" prop="categoryId" v-show="editItemIsShow(true, true)">
          <el-select  clearable  v-model="state.form.categoryId" placeholder="" >
            <el-option v-for="item in state.selectThingCategoryListData" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
        </el-form-item>
        <el-form-item label="标签" prop="tagIds_Values" v-show="editItemIsShow(true, true)">
          <el-select  clearable  multiple  v-model="state.form.tagIds_Values" placeholder="" >
            <el-option v-for="item in state.selectThingTagListData" :key="item.id" :value="String(item.id)" :label="item.name" />
          </el-select>
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

<script lang="ts" setup name="homely/thing/form">
import { reactive, toRefs, getCurrentInstance, ref, defineAsyncComponent} from 'vue'
import { ThingAddInput, ThingUpdateInput,
  ThingGetListInput, ThingGetListOutput,
  ThingCategoryGetListOutput,
  ThingCategoryGetOutput,
  ThingTagGetListOutput,
  ThingTagGetOutput,
} from '/@/api/homely/data-contracts'
import { ThingApi } from '/@/api/homely/Thing'
import { ThingCategoryApi } from '/@/api/homely/ThingCategory'
import { ThingTagApi } from '/@/api/homely/ThingTag'
import { auth, auths, authAll } from '/@/utils/authFunction'

 const MyUpload = defineAsyncComponent(() => import('/@/components/my-upload/index.vue'))      

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
  form: {} as ThingAddInput | ThingUpdateInput,
  selectThingCategoryListData: [] as ThingCategoryGetListOutput[],
  selectThingTagListData: [] as ThingTagGetListOutput[],

})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
    
  getThingCategoryList();
  getThingTagList();
  if (row.id > 0) {
    const res = await new ThingApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as ThingUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const getThingCategoryList = async () => {
  const res = await new ThingCategoryApi().getList({}).catch(() => {
    state.selectThingCategoryListData = []
  })
  state.selectThingCategoryListData = res?.data || []
}
const getThingTagList = async () => {
  const res = await new ThingTagApi().getList({}).catch(() => {
    state.selectThingTagListData = []
  })
  state.selectThingTagListData = res?.data || []
}

const defaultToAdd = (): ThingAddInput => {
  return {
    name: "",
    imageUrl: null,
    availableDate: null,
    remark: null,
    sort: null,
    categoryId: null,
    tagIds: null,
  } as ThingAddInput
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
      res = await new ThingApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new ThingApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshThing')
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
