<template>
<div class="my-layout">
    <el-card class="mt8 search-box" shadow="never">
      <el-row>
        <el-col :span="18">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item class="search-box-item">
              <el-input clearable  v-model="state.filter.inputText" placeholder="文本框" @keyup.enter="onQuery" >
              </el-input>
            </el-form-item>
            <el-form-item class="search-box-item">
              <el-select   clearable  v-model="state.filter.inputBussinessSingle" placeholder="模块业务单选" @keyup.enter="onQuery" >
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
                    <el-dropdown-item v-auth="perms.batSoftDelete" type="warning" :disabled="state.sels.length==0" @click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-space>
        </el-col>
      </el-row>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.codeGroupDemoListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" @selection-change="selsChange">
        
          <el-table-column type="selection" width="50" />
          <el-table-column prop="inputText" label="文本框" show-overflow-tooltip width />
          <el-table-column prop="inputNumber" label="数字框" show-overflow-tooltip width />
          <el-table-column prop="inputTextArea" label="文本域" show-overflow-tooltip width />
          <el-table-column prop="inputDate" label="日期" show-overflow-tooltip width />
          <el-table-column prop="inputSwitch" label="开关" show-overflow-tooltip width />
          <el-table-column prop="inputSelectCustom" label="下拉框" show-overflow-tooltip width />
          <el-table-column prop="inputCheckbox" label="复选框" show-overflow-tooltip width />
          <el-table-column prop="inputSelectDictDictName" label="字典" show-overflow-tooltip width />
          <el-table-column prop="inputBussinessSingle_Text" label="模块业务单选" show-overflow-tooltip width />
          <el-table-column prop="inputBussinessMultiple_Texts" label="模块业务多选" show-overflow-tooltip width >
            <template #default="{ row }">
              {{ row.inputBussinessMultiple_Texts ? row.inputBussinessMultiple_Texts.join(',') : '' }}
            </template>
          </el-table-column>
          <el-table-column prop="inputImage" label="图片上传" show-overflow-tooltip width >
            <template #default="{ row }">
             <div class="my-flex">
               <el-image :src="row.inputImage" :preview-src-list="previewInputImagelist"
                 :initial-index="getInputImageInitialIndex(row.inputImage)" :lazy="true" :hide-on-click-modal="true" fit="scale-down"
                 preview-teleported style="width: 80px; height: 80px" />
               <div class="ml10 my-flex-fill my-flex-y-center">
               </div>
             </div>
           </template>
          </el-table-column>
          <el-table-column prop="inputEditor" label="编辑器" show-overflow-tooltip width />
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

    <code-group-demo-form ref="codeGroupDemoFormRef" :title="state.codeGroupDemoFormTitle"></code-group-demo-form>
  </div>
</template>

<script lang="ts" setup name="dev/code-group-demo">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import { PageInputCodeGroupDemoGetPageInput, CodeGroupDemoGetPageInput, CodeGroupDemoGetPageOutput, CodeGroupDemoGetOutput, CodeGroupDemoAddInput, CodeGroupDemoUpdateInput,
  CodeGroupDemoGetListInput, CodeGroupDemoGetListOutput,
  CodeGroupGetListOutput,
  CodeGroupGetOutput,                    
} from '/@/api/dev/data-contracts'
import {  FileGetPageOutput } from '/@/api/admin/data-contracts'
import { CodeGroupDemoApi } from '/@/api/dev/CodeGroupDemo'
import { CodeGroupApi } from '/@/api/dev/CodeGroup'

import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const CodeGroupDemoForm = defineAsyncComponent(() => import('./components/code-group-demo-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const codeGroupDemoFormRef = ref()

//权限配置
const perms = {
  add:'api:dev:code-group-demo:add',
  update:'api:dev:code-group-demo:update',
  delete:'api:dev:code-group-demo:delete',
  batDelete:'api:dev:code-group-demo:batch-delete',
  softDelete:'api:dev:code-group-demo:soft-delete',
  batSoftDelete:'api:dev:code-group-demo:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  codeGroupDemoFormTitle: '',
  total: 0,
  sels: [] as Array<CodeGroupDemoGetPageOutput>,
  filter: {
    inputText: null,
    inputBussinessSingle: null,
  } as CodeGroupDemoGetPageInput | CodeGroupDemoGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputCodeGroupDemoGetPageInput,
  codeGroupDemoListData: [] as Array<CodeGroupDemoGetListOutput>,
  selectCodeGroupListData: [] as CodeGroupGetListOutput[],
  fileInputImageListData: [] as Array<FileGetPageOutput>,
})

onMounted(() => {

  getCodeGroupList();
  onQuery()
  eventBus.off('refreshCodeGroupDemo')
  eventBus.on('refreshCodeGroupDemo', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshCodeGroupDemo')
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
  const res = await new CodeGroupDemoApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.codeGroupDemoListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false

  state.fileInputImageListData = res?.data?.list?.map(s => {
    return { linkUrl: s.inputImage }
  }) ?? []


}

const onAdd = () => {
  state.codeGroupDemoFormTitle = '新增模板演示'
  codeGroupDemoFormRef.value.open()
}

const onEdit = (row: CodeGroupDemoGetOutput) => {
  state.codeGroupDemoFormTitle = '编辑模板演示'
  codeGroupDemoFormRef.value.open(row)
}

const onDelete = (row: CodeGroupDemoGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new CodeGroupDemoApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: CodeGroupDemoGetPageOutput[]) => {
  state.sels = vals
}


const onSoftDelete = async (row: CodeGroupDemoGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
    const rst = await new CodeGroupDemoApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
    const rst = await new CodeGroupDemoApi().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
    if(rst?.success){
      onQuery()
    }
  })
}
const previewInputImagelist = computed(() => {
  let imgList = [] as string[]
  state.fileInputImageListData.forEach((a) => {
    if (a.linkUrl) {
      imgList.push(a.linkUrl as string)
    }
  })
  return imgList
})
const getInputImageInitialIndex = (imgUrl: string) => {
  return previewInputImagelist.value.indexOf(imgUrl)
}
</script>
