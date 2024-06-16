<template>
  <div style="padding: 0px 8px">
    <!--查询-->
    <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
      <el-form inline :model="state.filterModel">
        <el-form-item>
          <el-input clearable  v-model="state.filterModel.name" placeholder="物品名称" @keyup.enter="onQuery" >
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-date-picker clearable  v-model="state.filterModel.availableDate" placeholder="有效期" @keyup.enter="onQuery" >
          </el-date-picker>
        </el-form-item>
        <el-form-item>
          <el-input clearable  v-model="state.filterModel.remark" placeholder="备注" @keyup.enter="onQuery" >
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery">查询</el-button>
        </el-form-item>
        <el-form-item v-auth="perms.add">
          <el-button type="primary" icon="ele-Plus" @click="onAdd">新增</el-button>
        </el-form-item>
        <el-form-item v-auths="[perms.batDelete, perms.batSoftDelete]" >
          <el-button v-auth="perms.batSoftDelete" type="warning" :disabled="state.sels.length==0" :placement="'bottom-end'" @click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-button>
        </el-form-item>
       </el-form>
     </el-card>

    <!--列表-->
    <el-card shadow="never" style="margin-top: 8px">
      <el-table size="small" v-loading="state.listLoading" :data="state.things" row-key="id" @selection-change="selsChange" >
        <template #empty>
          <el-empty :image-size="100" />
        </template>
        <el-table-column type="selection" width="50" />
        <el-table-column prop="name" label="物品名称" show-overflow-tooltip width />
        <el-table-column prop="imageUrl" label="图片" show-overflow-tooltip width />
        <el-table-column prop="availableDate" label="有效期" show-overflow-tooltip width />
        <el-table-column prop="remark" label="备注" show-overflow-tooltip width />
        <el-table-column prop="sort" label="排序" show-overflow-tooltip width />
        <el-table-column prop="categoryId" label="分类" show-overflow-tooltip width />
        <el-table-column prop="tags" label="标签" show-overflow-tooltip width />
        <el-table-column v-auths="[perms.update,perms.softDelete,perms.delete]" label="操作" :width="actionColWidth" fixed="right">
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button text type="warning" v-if="perms.softDelete" @click="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!--分页-->
      <div class="my-flex my-flex-end" style="margin-top: 20px">
        <el-pagination ref="pager" small background :total="state.total" :page-sizes="[10, 20, 50, 100]"
           v-model:currentPage="state.pageInput.currentPage"
           v-model:page-size="state.pageInput.pageSize"
           @size-change="onSizeChange" @current-change="onCurrentChange"
           layout="total, sizes, prev, pager, next, jumper"/>
      </div>
    </el-card>

    
    <el-drawer direction="rtl" v-model="state.formShow" :title="state.formTitle">
      <el-form :model="state.formData" label-width="100" style="margin:8px;"
        :rules="state.editMode=='add'?addRules:updateRules" ref="dataEditor">
        <el-form-item label="物品名称" prop="name" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.name" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="图片" prop="imageUrl" v-show="editItemIsShow(true, true)">
          <my-upload  v-if='state.formShow'  v-model="state.formData.imageUrl" placeholder="" >
          </my-upload>
        </el-form-item>
        <el-form-item label="有效期" prop="availableDate" v-show="editItemIsShow(true, true)">
          <el-date-picker  v-model="state.formData.availableDate" placeholder="" >
          </el-date-picker>
        </el-form-item>
        <el-form-item label="备注" prop="remark" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.remark" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="排序" prop="sort" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.sort" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="分类" prop="categoryId" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.categoryId" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="标签" prop="tags" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.tags" placeholder="" >
          </el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-card>
          <el-button @click="state.formShow = false">取消</el-button>
          <el-button type="primary" @click="submitData(state.formData)">确定</el-button>
        </el-card>
      </template>
    </el-drawer>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted, getCurrentInstance, onUnmounted, defineAsyncComponent } from 'vue'
import { FormRules } from 'element-plus'
import { PageInputThingGetPageInput, ThingGetPageInput, ThingGetPageOutput, ThingGetOutput, ThingAddInput, ThingUpdateInput,
  ThingGetListInput, ThingGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingApi } from '/@/api/homely/Thing'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

const MyUpload = defineAsyncComponent(() => import('/@/components/my-upload/index.vue'))      

const { proxy } = getCurrentInstance() as any

const dataEditor = ref()

const perms = {
  add:'api:homely:thing:add',
  update:'api:homely:thing:update',
  delete:'api:homely:thing:delete',
  batDelete:'api:homely:thing:batch-delete',
  softDelete:'api:homely:thing:soft-delete',
  batSoftDelete:'api:homely:thing:batch-soft-delete',
}

const actionColWidth = authAll([perms.delete,perms.softDelete])?125:auths([perms.delete,perms.softDelete])?135:70;

const formRules = reactive<FormRules>({
  name:[{ required: true, message: '物品名称不能为空！', trigger: 'blur'}],
})

const addRules = {
  name: formRules.name,
}
const updateRules = {
  name: formRules.name,
}

const state = reactive({
  listLoading: false,
  formTitle: '',
  editMode: 'add',
  formShow: false,
  formData: {} as ThingAddInput | ThingUpdateInput,
  sels: [] as Array<ThingGetPageOutput>,
  filterModel: {
    name: null,
    availableDate: null,
    remark: null,
  } as ThingGetPageInput | ThingGetListInput,
  total:0,
  pageInput:{
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingGetPageInput,
  things: [] as Array<ThingGetPageOutput>,
  thingList: [] as Array<ThingGetListOutput>,
})

const editItemIsShow = (add: Boolean, edit: Boolean): Boolean => {
    if(add && edit)return true;
    if(add && state.editMode == 'add')return true;
    if(edit && state.editMode == 'edit')return true;
    return false;
}

onMounted(()=>{
  onQuery()
})

onUnmounted(()=>{

})


const showEditor = () => {
  state.formShow = true
  dataEditor?.value?.resetFields()
}

const defaultToAdd = (): ThingAddInput => {
  return {
    name: "",
    imageUrl: null,
    availableDate: null,
    remark: null,
    sort: null,
    categoryId: null,
    tags: null,
  } as ThingAddInput
}

const onQuery = async () => {
  state.listLoading = true
  
  var queryParams = state.pageInput;
  queryParams.filter = state.filterModel;
  queryParams.dynamicFilter = {};

  const res = await new ThingApi().getPage(queryParams)

  state.things = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.listLoading = false
}
const onSizeChange = (val: number) => {
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

const selsChange = (vals: ThingGetPageOutput[]) => {
  state.sels = vals
}

const onAdd = () => {
  state.editMode = 'add'
  state.formTitle = '新增物品'
  state.formData = defaultToAdd()
  showEditor()
}

const onEdit = async (row: ThingGetOutput) => {
  state.editMode = 'edit'
  state.formTitle = '编辑物品'
  const res = await new ThingApi().get({id: row.id}, { loading: true})
  if (res?.success) {
    showEditor()
    state.formData = res.data as ThingUpdateInput
  }
}

const onDelete = async (row: ThingGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要删除？`).then(async () =>{
      const rst = await new ThingApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      if(rst?.success){
        onQuery()
      }
    })
}

const onAddPost = async (addData: ThingAddInput) => {
  const res = await new ThingApi().add(addData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const onUpdatePost = async (updateData: ThingUpdateInput) => {
  const res = await new ThingApi().update(updateData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const submitData = async (editData: ThingAddInput | ThingUpdateInput) => {
  dataEditor?.value?.validate(async (valid: boolean) => {
    if (!valid) return
    
    if (state.editMode == 'add') {
      onAddPost(editData)
    } else if (state.editMode == 'edit') {
      onUpdatePost(editData)
    }
  })
}


const onSoftDelete = async (row: ThingGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new ThingApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
    const rst = await new ThingApi().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}
</script>

<script lang="ts">
import { defineComponent } from 'vue'
export default defineComponent({
  name: 'homely/thing'
})
</script>