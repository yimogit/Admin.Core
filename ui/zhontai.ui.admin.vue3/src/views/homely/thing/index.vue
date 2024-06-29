<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item class="search-box-item"  label="物品名称">
              <el-input  clearable  v-model="state.filter.name" placeholder="" @keyup.enter="onQuery" >
              </el-input>
            </el-form-item>
            <el-form-item class="search-box-item"  label="有效期">
              <el-date-picker  clearable  v-model="state.filter.availableDate" placeholder="" @keyup.enter="onQuery" >
              </el-date-picker>
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
      <el-table v-loading="state.loading" :data="state.thingListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
          <el-table-column type="selection" width="50" />
          <el-table-column prop="name" label="物品名称" show-overflow-tooltip width />
          <el-table-column prop="categoryId_Text" label="分类" show-overflow-tooltip width />
          <el-table-column prop="availableDate" label="有效期" show-overflow-tooltip width />
          <el-table-column prop="sort" label="排序" show-overflow-tooltip width />
          <el-table-column prop="tagIds_Texts" label="标签" show-overflow-tooltip width >
            <template #default="{ row }">
              {{ row.tagIds_Texts ? row.tagIds_Texts.join(',') : '' }}
            </template>
          </el-table-column>
          <el-table-column prop="remark" label="备注" show-overflow-tooltip width />
          <el-table-column prop="imageUrl" label="图片" show-overflow-tooltip width >
            <template #default="{ row }">
             <div class="my-flex">
               <el-image :src="row.imageUrl" :preview-src-list="previewImageUrllist"
                 :initial-index="getImageUrlInitialIndex(row.imageUrl)" :lazy="true" :hide-on-click-modal="true" fit="scale-down"
                 preview-teleported style="width: 80px; height: 80px" />
               <div class="ml10 my-flex-fill my-flex-y-center">
               </div>
             </div>
           </template>
          </el-table-column>
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
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInputThingGetPageInput, ThingGetPageInput, ThingGetPageOutput, ThingGetOutput, ThingAddInput, ThingUpdateInput,
  ThingGetListInput, ThingGetListOutput,
  ThingCategoryGetListOutput,
  ThingCategoryGetOutput,                    
  ThingTagGetListOutput,
  ThingTagGetOutput,                    
} from '/@/api/homely/data-contracts'
import {  FileGetPageOutput } from '/@/api/admin/data-contracts'
import { ThingApi } from '/@/api/homely/Thing'
import { ThingCategoryApi } from '/@/api/homely/ThingCategory'
import { ThingTagApi } from '/@/api/homely/ThingTag'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const ThingForm = defineAsyncComponent(() => import('./components/thing-form.vue'))

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
  } as ThingGetPageInput | ThingGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputThingGetPageInput,
  thingListData: [] as Array<ThingGetListOutput>,
  selectThingCategoryListData: [] as ThingCategoryGetListOutput[],
  selectThingTagListData: [] as ThingTagGetListOutput[],
  fileImageUrlListData: [] as Array<FileGetPageOutput>,
})

onMounted(() => {

  getThingCategoryList();
  getThingTagList();
  onQuery()
  eventBus.off('refreshThing')
  eventBus.on('refreshThing', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshThing')
})

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


const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new ThingApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.thingListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false

  state.fileImageUrlListData = res?.data?.list?.map(s => {
    return { linkUrl: s.imageUrl }
  }) ?? []


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
const previewImageUrllist = computed(() => {
  let imgList = [] as string[]
  state.fileImageUrlListData.forEach((a) => {
    if (a.linkUrl) {
      imgList.push(a.linkUrl as string)
    }
  })
  return imgList
})
const getImageUrlInitialIndex = (imgUrl: string) => {
  return previewImageUrllist.value.indexOf(imgUrl)
}
</script>
