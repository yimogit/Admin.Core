<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item class="search-box-item">
              <el-select clearable  v-model="state.filter.groupId" placeholder="模板分组" @keyup.enter="onQuery" >
                <el-option v-for="item in state.selectCodeGroupListData" :key="item.id" :value="item.id" :label="item.title" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onQuery">查询</el-button>
            </el-form-item>
          </el-form>
        </el-col>
        <el-col :span="6" class="text-right">
          <el-space>
          <el-button type="primary" icon="ele-Plus" @click="onAdd">新增</el-button>
            <el-dropdown :placement="'bottom-end'">
              <el-button type="warning">批量操作 <el-icon><ele-ArrowDown /></el-icon></el-button>
              <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item v-auth="perms.batSoftDelete" :disabled="state.sels.length==0" @click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-space>
        </el-col>
      </el-row>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.codeGroupDetailListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
          <el-table-column type="selection" width="50" />
          <el-table-column prop="name" label="模板名称" show-overflow-tooltip width />
          <el-table-column prop="groupId_Text" label="模板分组" show-overflow-tooltip width />
          <el-table-column prop="path" label="生成路径" show-overflow-tooltip width />
          <el-table-column prop="groupIds_Texts" label="模板分组" show-overflow-tooltip width >
            <template #default="{ row }">
              {{ row.groupIds_Texts ? row.groupIds_Texts.join(',') : '' }}
            </template>
          </el-table-column>
          <el-table-column prop="content" label="模板内容" show-overflow-tooltip width />
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

    <code-group-detail-form ref="codeGroupDetailFormRef" :title="state.codeGroupDetailFormTitle"></code-group-detail-form>
  </div>
</template>

<script lang="ts" setup name="dev/code-group-detail">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInputCodeGroupDetailGetPageInput, CodeGroupDetailGetPageInput, CodeGroupDetailGetPageOutput, CodeGroupDetailGetOutput, CodeGroupDetailAddInput, CodeGroupDetailUpdateInput,
  CodeGroupDetailGetListInput, CodeGroupDetailGetListOutput,
  CodeGroupGetListOutput,
  CodeGroupGetOutput,                    
} from '/@/api/dev/data-contracts'
import { CodeGroupDetailApi } from '/@/api/dev/CodeGroupDetail'
import { CodeGroupApi } from '/@/api/dev/CodeGroup'

import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const CodeGroupDetailForm = defineAsyncComponent(() => import('./components/code-group-detail-form.vue'))

const { proxy } = getCurrentInstance() as any

const codeGroupDetailFormRef = ref()

//权限配置
const perms = {
  add:'api:dev:code-group-detail:add',
  update:'api:dev:code-group-detail:update',
  delete:'api:dev:code-group-detail:delete',
  batDelete:'api:dev:code-group-detail:batch-delete',
  softDelete:'api:dev:code-group-detail:soft-delete',
  batSoftDelete:'api:dev:code-group-detail:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  codeGroupDetailFormTitle: '',
  total: 0,
  sels: [] as Array<CodeGroupDetailGetPageOutput>,
  filter: {
    groupId: null,
  } as CodeGroupDetailGetPageInput | CodeGroupDetailGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputCodeGroupDetailGetPageInput,
  codeGroupDetailListData: [] as Array<CodeGroupDetailGetListOutput>,
  selectCodeGroupListData: [] as CodeGroupGetListOutput[],
})

onMounted(() => {

  getCodeGroupList();
  onQuery()
  eventBus.off('refreshCodeGroupDetail')
  eventBus.on('refreshCodeGroupDetail', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshCodeGroupDetail')
})

const getCodeGroupList = async () => {
  const res = await new CodeGroupApi().getList({}).catch(() => {
    state.selectCodeGroupListData = []
  })
  state.selectCodeGroupListData = res?.data || []
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new CodeGroupDetailApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.codeGroupDetailListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false



}

const onAdd = () => {
  state.codeGroupDetailFormTitle = '新增模板明细'
  codeGroupDetailFormRef.value.open()
}

const onEdit = (row: CodeGroupDetailGetOutput) => {
  state.codeGroupDetailFormTitle = '编辑模板明细'
  codeGroupDetailFormRef.value.open(row)
}

const onDelete = (row: CodeGroupDetailGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new CodeGroupDetailApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: CodeGroupDetailGetPageOutput[]) => {
  state.sels = vals
}


const onSoftDelete = async (row: CodeGroupDetailGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new CodeGroupDetailApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
    const rst = await new CodeGroupDetailApi().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}
</script>
