<template>
  <div style="padding: 0px 8px">
    <!--查询-->
    <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
      <el-form inline :model="state.filterModel">
        <el-form-item>
          <el-input clearable  v-model="state.filterModel.name" placeholder="分类名称" @keyup.enter="onQuery" >
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
      <el-table size="small" v-loading="state.listLoading" :data="state.thingCategorys" row-key="id" @selection-change="selsChange" >
        <template #empty>
          <el-empty :image-size="100" />
        </template>
        <el-table-column type="selection" width="50" />
        <el-table-column prop="sort" label="排序" show-overflow-tooltip width />
        <el-table-column prop="name" label="分类名称" show-overflow-tooltip width />
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
        <el-form-item label="排序" prop="sort" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.sort" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="分类名称" prop="name" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.name" placeholder="" >
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
import { PageInputThingCategoryGetPageInput, ThingCategoryGetPageInput, ThingCategoryGetPageOutput, ThingCategoryGetOutput, ThingCategoryAddInput, ThingCategoryUpdateInput,
  ThingCategoryGetListInput, ThingCategoryGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingCategoryApi } from '/@/api/homely/ThingCategory'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'


const { proxy } = getCurrentInstance() as any

const dataEditor = ref()

const perms = {
  add:'api:homely:thing-category:add',
  update:'api:homely:thing-category:update',
  delete:'api:homely:thing-category:delete',
  batDelete:'api:homely:thing-category:batch-delete',
  softDelete:'api:homely:thing-category:soft-delete',
  batSoftDelete:'api:homely:thing-category:batch-soft-delete',
}

const actionColWidth = authAll([perms.delete,perms.softDelete])?125:auths([perms.delete,perms.softDelete])?135:70;

const formRules = reactive<FormRules>({
})

const addRules = {
}
const updateRules = {
}

const state = reactive({
  listLoading: false,
  formTitle: '',
  editMode: 'add',
  formShow: false,
  formData: {} as ThingCategoryAddInput | ThingCategoryUpdateInput,
  sels: [] as Array<ThingCategoryGetPageOutput>,
  filterModel: {
    name: null,
  } as ThingCategoryGetPageInput | ThingCategoryGetListInput,
  total:0,
  pageInput:{
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingCategoryGetPageInput,
  thingCategorys: [] as Array<ThingCategoryGetPageOutput>,
  thingCategoryList: [] as Array<ThingCategoryGetListOutput>,
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

const defaultToAdd = (): ThingCategoryAddInput => {
  return {
    sort: null,
    name: null,
    tenantId: null,
  } as ThingCategoryAddInput
}

const onQuery = async () => {
  state.listLoading = true
  
  var queryParams = state.pageInput;
  queryParams.filter = state.filterModel;
  queryParams.dynamicFilter = {};

  const res = await new ThingCategoryApi().getPage(queryParams)

  state.thingCategorys = res?.data?.list ?? []
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

const selsChange = (vals: ThingCategoryGetPageOutput[]) => {
  state.sels = vals
}

const onAdd = () => {
  state.editMode = 'add'
  state.formTitle = '新增物品分类'
  state.formData = defaultToAdd()
  showEditor()
}

const onEdit = async (row: ThingCategoryGetOutput) => {
  state.editMode = 'edit'
  state.formTitle = '编辑物品分类'
  const res = await new ThingCategoryApi().get({id: row.id}, { loading: true})
  if (res?.success) {
    showEditor()
    state.formData = res.data as ThingCategoryUpdateInput
  }
}

const onDelete = async (row: ThingCategoryGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要删除？`).then(async () =>{
      const rst = await new ThingCategoryApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      if(rst?.success){
        onQuery()
      }
    })
}

const onAddPost = async (addData: ThingCategoryAddInput) => {
  const res = await new ThingCategoryApi().add(addData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const onUpdatePost = async (updateData: ThingCategoryUpdateInput) => {
  const res = await new ThingCategoryApi().update(updateData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const submitData = async (editData: ThingCategoryAddInput | ThingCategoryUpdateInput) => {
  dataEditor?.value?.validate(async (valid: boolean) => {
    if (!valid) return
    
    if (state.editMode == 'add') {
      onAddPost(editData)
    } else if (state.editMode == 'edit') {
      onUpdatePost(editData)
    }
  })
}


const onSoftDelete = async (row: ThingCategoryGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new ThingCategoryApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
    const rst = await new ThingCategoryApi().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}
</script>

<script lang="ts">
import { defineComponent } from 'vue'
export default defineComponent({
  name: 'homely/thingCategory'
})
</script>