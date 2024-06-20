<template>
<div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item>
          <el-input clearable  v-model="state.filter.name" placeholder="物品名称" @keyup.enter="onQuery" >
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-date-picker clearable  v-model="state.filter.availableDate" placeholder="有效期" @keyup.enter="onQuery" >
          </el-date-picker>
        </el-form-item>
        <el-form-item>
          <el-input clearable  v-model="state.filter.remark" placeholder="备注" @keyup.enter="onQuery" >
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery">查询</el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="onAdd">新增</el-button>
        </el-form-item>
          <el-form-item v-auths="[perms.batDelete, perms.batSoftDelete]" >
            <el-button v-auth="perms.batSoftDelete" type="warning" :disabled="state.sels.length==0" :placement="'bottom-end'" @click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-button>
          </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.thingListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
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

    <thing-form ref="thingFormRef" :title="state.thingFormTitle"></thing-form>
  </div>
</template>

<script lang="ts" setup name="homely/thing">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { PageInputThingGetPageInput, ThingGetPageInput, ThingGetPageOutput, ThingGetOutput, ThingAddInput, ThingUpdateInput,
  ThingGetListInput, ThingGetListOutput,
} from '/@/api/homely/data-contracts'
import { ThingApi } from '/@/api/homely/Thing'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const ThingForm = defineAsyncComponent(() => import('./components/thing-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const thingFormRef = ref()

//权限配置
const perms = {
  add:'api:homely:thing:add',
  update:'api:homely:thing:update',
  delete:'api:homely:thing:delete',
  batDelete:'api:homely:thing:batch-delete',
  softDelete:'api:homely:thing:soft-delete',
  batSoftDelete:'api:homely:thing:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  thingFormTitle: '',
  total: 0,
  sels: [] as Array<ThingGetPageOutput>,
  filter: {
    name: null,
    availableDate: null,
    remark: null,
  } as ThingGetPageInput | ThingGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingGetPageInput,
  thingListData: [] as Array<ThingGetListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshThing')
  eventBus.on('refreshThing', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshThing')
})

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new ThingApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.thingListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.thingFormTitle = '新增物品'
  thingFormRef.value.open()
}

const onEdit = (row: ThingGetOutput) => {
  state.thingFormTitle = '编辑物品'
  thingFormRef.value.open(row)
}

const onDelete = (row: ThingGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new ThingApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: ThingGetPageOutput[]) => {
  state.sels = vals
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
