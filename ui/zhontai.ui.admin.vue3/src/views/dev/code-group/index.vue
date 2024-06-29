<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item class="search-box-item"  label="模板标题">
              <el-input  clearable  v-model="state.filter.name" placeholder="" @keyup.enter="onQuery" >
              </el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onQuery">查询</el-button>
            </el-form-item>
          </el-form>
        </el-col>
        <el-col :span="6" class="text-right">
          <el-space>
          <el-button type="primary" icon="ele-Plus" @click="onAdd">新增</el-button>
          </el-space>
        </el-col>
      </el-row>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.codeGroupListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
          <el-table-column type="selection" width="50" />
          <el-table-column prop="name" label="模板标题" show-overflow-tooltip width />
          <el-table-column prop="remark" label="备注" show-overflow-tooltip width />
          <el-table-column v-auths="[perms.update,perms.softDelete,perms.delete]" label="操作" :width="actionColWidth" fixed="right">
            <template #default="{ row }">
              <el-button v-auth="perms.update" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
              <el-button text type="warning" v-if="perms.softDelete" @click="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-button>
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
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <code-group-form ref="codeGroupFormRef" :title="state.codeGroupFormTitle"></code-group-form>
  </div>
</template>

<script lang="ts" setup name="dev/code-group">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInputCodeGroupGetPageInput, CodeGroupGetPageInput, CodeGroupGetPageOutput, CodeGroupGetOutput, CodeGroupAddInput, CodeGroupUpdateInput,
  CodeGroupGetListInput, CodeGroupGetListOutput,
} from '/@/api/dev/data-contracts'
import { CodeGroupApi } from '/@/api/dev/CodeGroup'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const CodeGroupForm = defineAsyncComponent(() => import('./components/code-group-form.vue'))

const { proxy } = getCurrentInstance() as any

const codeGroupFormRef = ref()

//权限配置
const perms = {
  add:'api:dev:code-group:add',
  update:'api:dev:code-group:update',
  delete:'api:dev:code-group:delete',
  batDelete:'api:dev:code-group:batch-delete',
  softDelete:'api:dev:code-group:soft-delete',
  batSoftDelete:'api:dev:code-group:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  codeGroupFormTitle: '',
  total: 0,
  sels: [] as Array<CodeGroupGetPageOutput>,
  filter: {
    name: null,
  } as CodeGroupGetPageInput | CodeGroupGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputCodeGroupGetPageInput,
  codeGroupListData: [] as Array<CodeGroupGetListOutput>,
})

onMounted(() => {

  onQuery()
  eventBus.off('refreshCodeGroup')
  eventBus.on('refreshCodeGroup', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshCodeGroup')
})



const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new CodeGroupApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.codeGroupListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false



}

const onAdd = () => {
  state.codeGroupFormTitle = '新增模板组'
  codeGroupFormRef.value.open()
}

const onEdit = (row: CodeGroupGetOutput) => {
  state.codeGroupFormTitle = '编辑模板组'
  codeGroupFormRef.value.open(row)
}

const onDelete = (row: CodeGroupGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new CodeGroupApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: CodeGroupGetPageOutput[]) => {
  state.sels = vals
}


const onSoftDelete = async (row: CodeGroupGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new CodeGroupApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

</script>
