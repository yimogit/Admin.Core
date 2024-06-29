<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item class="search-box-item"  label="分类名称">
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
      <el-table v-loading="state.loading" :data="state.thingCategoryListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
          <el-table-column type="selection" width="50" />
          <el-table-column prop="name" label="分类名称" show-overflow-tooltip width />
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

    <thing-category-form ref="thingCategoryFormRef" :title="state.thingCategoryFormTitle"></thing-category-form>
  </div>
</template>

<script lang="ts" setup name="homely/thing-category">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInputThingCategoryGetPageInput, ThingCategoryGetPageInput, ThingCategoryGetPageOutput, ThingCategoryGetOutput, ThingCategoryAddInput, ThingCategoryUpdateInput,
  ThingCategoryGetListInput, ThingCategoryGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingCategoryApi } from '/@/api/homely/ThingCategory'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const ThingCategoryForm = defineAsyncComponent(() => import('./components/thing-category-form.vue'))

const { proxy } = getCurrentInstance() as any

const thingCategoryFormRef = ref()

//权限配置
const perms = {
  add:'api:homely:thing-category:add',
  update:'api:homely:thing-category:update',
  delete:'api:homely:thing-category:delete',
  batDelete:'api:homely:thing-category:batch-delete',
  softDelete:'api:homely:thing-category:soft-delete',
  batSoftDelete:'api:homely:thing-category:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  thingCategoryFormTitle: '',
  total: 0,
  sels: [] as Array<ThingCategoryGetPageOutput>,
  filter: {
    name: null,
  } as ThingCategoryGetPageInput | ThingCategoryGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingCategoryGetPageInput,
  thingCategoryListData: [] as Array<ThingCategoryGetListOutput>,
})

onMounted(() => {

  onQuery()
  eventBus.off('refreshThingCategory')
  eventBus.on('refreshThingCategory', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshThingCategory')
})



const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new ThingCategoryApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.thingCategoryListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false



}

const onAdd = () => {
  state.thingCategoryFormTitle = '新增物品分类'
  thingCategoryFormRef.value.open()
}

const onEdit = (row: ThingCategoryGetOutput) => {
  state.thingCategoryFormTitle = '编辑物品分类'
  thingCategoryFormRef.value.open(row)
}

const onDelete = (row: ThingCategoryGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new ThingCategoryApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: ThingCategoryGetPageOutput[]) => {
  state.sels = vals
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
