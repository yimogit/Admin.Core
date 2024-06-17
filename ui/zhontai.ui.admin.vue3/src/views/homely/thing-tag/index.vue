<template>
  <div style="padding: 0px 8px">
    <!--查询-->
    <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
      <el-form inline :model="state.filterModel">
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
      <el-table size="small" v-loading="state.listLoading" :data="state.thingTags" row-key="id" @selection-change="selsChange" >
        <template #empty>
          <el-empty :image-size="100" />
        </template>
        <el-table-column type="selection" width="50" />
        <el-table-column prop="name" label="标签名称" show-overflow-tooltip width />
        <el-table-column prop="sort" label="排序" show-overflow-tooltip width />
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
        <el-form-item label="标签名称" prop="name" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.name" placeholder="" >
          </el-input>
        </el-form-item>
        <el-form-item label="排序" prop="sort" v-show="editItemIsShow(true, true)">
          <el-input  v-model="state.formData.sort" placeholder="" >
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
import { PageInputThingTagGetPageInput, ThingTagGetPageInput, ThingTagGetPageOutput, ThingTagGetOutput, ThingTagAddInput, ThingTagUpdateInput,
} from '/@/api/homely/data-contracts'
import { ThingTagApi } from '/@/api/homely/ThingTag'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'


const { proxy } = getCurrentInstance() as any

const dataEditor = ref()

const perms = {
  add:'api:homely:thing-tag:add',
  update:'api:homely:thing-tag:update',
  delete:'api:homely:thing-tag:delete',
  batDelete:'api:homely:thing-tag:batch-delete',
  softDelete:'api:homely:thing-tag:soft-delete',
  batSoftDelete:'api:homely:thing-tag:batch-soft-delete',
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
  formData: {} as ThingTagAddInput | ThingTagUpdateInput,
  sels: [] as Array<ThingTagGetPageOutput>,
  filterModel: {
  } as ThingTagGetPageInput ,
  total:0,
  pageInput:{
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingTagGetPageInput,
  thingTags: [] as Array<ThingTagGetPageOutput>,
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

const defaultToAdd = (): ThingTagAddInput => {
  return {
    name: null,
    sort: null,
  } as ThingTagAddInput
}

const onQuery = async () => {
  state.listLoading = true
  
  var queryParams = state.pageInput;
  queryParams.filter = state.filterModel;
  queryParams.dynamicFilter = {};

  const res = await new ThingTagApi().getPage(queryParams)

  state.thingTags = res?.data?.list ?? []
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

const selsChange = (vals: ThingTagGetPageOutput[]) => {
  state.sels = vals
}

const onAdd = () => {
  state.editMode = 'add'
  state.formTitle = '新增标签'
  state.formData = defaultToAdd()
  showEditor()
}

const onEdit = async (row: ThingTagGetOutput) => {
  state.editMode = 'edit'
  state.formTitle = '编辑标签'
  const res = await new ThingTagApi().get({id: row.id}, { loading: true})
  if (res?.success) {
    showEditor()
    state.formData = res.data as ThingTagUpdateInput
  }
}

const onDelete = async (row: ThingTagGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要删除？`).then(async () =>{
      const rst = await new ThingTagApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      if(rst?.success){
        onQuery()
      }
    })
}

const onAddPost = async (addData: ThingTagAddInput) => {
  const res = await new ThingTagApi().add(addData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const onUpdatePost = async (updateData: ThingTagUpdateInput) => {
  const res = await new ThingTagApi().update(updateData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const submitData = async (editData: ThingTagAddInput | ThingTagUpdateInput) => {
  dataEditor?.value?.validate(async (valid: boolean) => {
    if (!valid) return
    
    if (state.editMode == 'add') {
      onAddPost(editData)
    } else if (state.editMode == 'edit') {
      onUpdatePost(editData)
    }
  })
}


const onSoftDelete = async (row: ThingTagGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new ThingTagApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
    const rst = await new ThingTagApi().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}
</script>

<script lang="ts">
import { defineComponent } from 'vue'
export default defineComponent({
  name: 'homely/thingTag'
})
</script>