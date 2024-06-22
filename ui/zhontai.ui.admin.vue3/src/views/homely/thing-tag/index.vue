<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="onAdd">新增</el-button>
        </el-form-item>
          <el-form-item v-auths="[perms.batDelete, perms.batSoftDelete]" >
            <el-button v-auth="perms.batSoftDelete" type="warning" :disabled="state.sels.length==0" :placement="'bottom-end'" @click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-button>
          </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.thingTagListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
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

    <thing-tag-form ref="thingTagFormRef" :title="state.thingTagFormTitle"></thing-tag-form>
  </div>
</template>

<script lang="ts" setup name="homely/thing-tag">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { PageInputThingTagGetPageInput, ThingTagGetPageInput, ThingTagGetPageOutput, ThingTagGetOutput, ThingTagAddInput, ThingTagUpdateInput,
  ThingTagGetListInput, ThingTagGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingTagApi } from '/@/api/homely/ThingTag'

import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const ThingTagForm = defineAsyncComponent(() => import('./components/thing-tag-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const thingTagFormRef = ref()

//权限配置
const perms = {
  add:'api:homely:thing-tag:add',
  update:'api:homely:thing-tag:update',
  delete:'api:homely:thing-tag:delete',
  batDelete:'api:homely:thing-tag:batch-delete',
  softDelete:'api:homely:thing-tag:soft-delete',
  batSoftDelete:'api:homely:thing-tag:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  thingTagFormTitle: '',
  total: 0,
  sels: [] as Array<ThingTagGetPageOutput>,
  filter: {
  } as ThingTagGetPageInput | ThingTagGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingTagGetPageInput,
  thingTagListData: [] as Array<ThingTagGetListOutput>,
})

onMounted(() => {

  onQuery()
  eventBus.off('refreshThingTag')
  eventBus.on('refreshThingTag', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshThingTag')
})


const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new ThingTagApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.thingTagListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.thingTagFormTitle = '新增物品标签'
  thingTagFormRef.value.open()
}

const onEdit = (row: ThingTagGetOutput) => {
  state.thingTagFormTitle = '编辑物品标签'
  thingTagFormRef.value.open(row)
}

const onDelete = (row: ThingTagGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new ThingTagApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: ThingTagGetPageOutput[]) => {
  state.sels = vals
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
