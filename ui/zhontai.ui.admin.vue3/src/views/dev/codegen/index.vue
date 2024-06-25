<template>
  <div>
    <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
      <el-form class="ad-form-query" inline :model="state.filter">
        <el-form-item label="数据源">
          <el-select v-model="state.filter.dbKey" @change="getConfigs" style="width: 150px" clearable>
            <el-option v-for="item in state.dbKeys" :key="item.dbKey" :value="item.dbKey" :label="item.dbKey"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-dropdown split-button type="primary" @click="createTable">
              创建表
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item type="primary" @click="importConfig">从剪切板导入配置</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="getTables">查看数据库结构</el-button>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-dropdown split-button type="primary" @click="exportConfig">
              复制选中配置到剪切板
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item type="primary" @click="importConfig">从剪切板导入配置</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-button-group>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card shadow="never" style="margin-top: 8px">
      <el-table :data="state.configs" highlight-current-row size="default" @row-click="configSelect"
        @row-dblclick="modifyConfig" @selection-change="selsChange">
        <el-table-column type="selection" width="50" />
        <el-table-column type="expand" fixed>
          <template #default="scope">
            <el-row>
              <el-col :span="12">后端输出位置：{{ scope.row.backendOut }}</el-col>
              <el-col :span="12">前端输出位置：{{ scope.row.frontendOut }}</el-col>
            </el-row>
          </template>
        </el-table-column>
        <el-table-column prop="tableName" label="表名称" width="180" fixed></el-table-column>
        <el-table-column prop="entityName" label="实体名" width="140" fixed></el-table-column>
        <el-table-column prop="namespace" label="命名空间" width="180"></el-table-column>
        <el-table-column prop="busName" label="业务名" width="100"></el-table-column>
        <el-table-column prop="baseEntity" label="基类" width="120"></el-table-column>
        <el-table-column prop="apiAreaName" label="Api分区" width="100"></el-table-column>
        <el-table-column prop="generateType" label="生成方式" width="100">
          <template #default="scope">
            {{ genType(scope.row.generateType) }}
          </template>
        </el-table-column>
        <el-table-column prop="authorName" label="作者" width="100"></el-table-column>
        <el-table-column prop="comment" label="备注" width></el-table-column>
        <el-table-column label="操作" width="180" fixed="right" header-align="center">
          <template #default="scope">
            <el-space wrap :size="5">
              <el-dropdown split-button type="primary" size="small" @click="modifyConfig(scope.row)">
                编辑
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item @click="generate(scope.row)">生成代码</el-dropdown-item>
                    <el-dropdown-item @click="compile(scope.row)">生成迁移SQL</el-dropdown-item>
                    <el-dropdown-item @click="genCompile(scope.row)">执行迁移到数据库</el-dropdown-item>
                    <el-dropdown-item @click="genMenu(scope.row)">生成菜单数据</el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
              <el-button type="danger" split-button size="small" icon="ele-Delete"
                @click="removeConfig(scope.row)">删除</el-button>
            </el-space>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <codegen-form ref="codegenFormRef" @sure="onConfigEditSure"></codegen-form>
    <el-drawer ref="tablesDrawerRef" v-model="state.dbStructShow" :title="state.filter.dbKey + ' 数据库结构'" direction="rtl">
      <el-tree :data="state.dbTree" @node-click="tableNodeSelect" />
      <template #footer>
        <span style="margin: 8px">
          <el-button @click="state.dbStructShow = false">取消</el-button>
          <el-button @click="createConfigFromTable(state.filter.dbTree)" type="primary"
            :disabled="!state.filter.dbTree || (state.filter.dbTree != null && state.filter.dbTree.type != 'table')">
            按表结构创建生成配置
          </el-button>
        </span>
      </template>
    </el-drawer>
    <el-dialog title="导入确认" destroy-on-close draggable width="80%" class="dialog-h50" :close-on-click-modal="false"
      v-model="state.importBoxyShow">
      <el-table :data="state.importConfigs" highlight-current-row size="default" @selection-change="importSelsChange"
        ref="importDialogTableRef">
        <el-table-column type="selection" width="50" />
        <el-table-column prop="tableName" label="表名称" width="180" fixed></el-table-column>
        <el-table-column prop="entityName" label="实体名" width="140" fixed></el-table-column>
        <el-table-column prop="namespace" label="命名空间" width="180"></el-table-column>
        <el-table-column prop="busName" label="业务名" width="100"></el-table-column>
        <el-table-column prop="baseEntity" label="基类" width="120"></el-table-column>
        <el-table-column prop="apiAreaName" label="Api分区" width="100"></el-table-column>
        <el-table-column prop="generateType" label="生成方式" width="100">
          <template #default="scope">
            {{ genType(scope.row.generateType) }}
          </template>
        </el-table-column>
        <el-table-column prop="authorName" label="作者" width="100"></el-table-column>
        <el-table-column prop="comment" label="备注" width></el-table-column>
        <el-table-column prop="importStatus" label="导入状态" width show-overflow-tooltip></el-table-column>
      </el-table>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancelImport"> 取消 </el-button>
          <el-button type="primary" @click="onSureImport"> {{ state.importSuccess ? '导入完成' : '确定' }}</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>



<script lang="ts" setup>
import { ref, reactive, onMounted, getCurrentInstance, onUnmounted, defineAsyncComponent, toRaw } from 'vue'
import { BaseDataGetOutput, DatabaseGetOutput, CodeGenGetOutput, CodeGenUpdateInput, CodeGenFieldGetOutput } from '/@/api/dev/data-contracts'
import { CodeGenApi } from '/@/api/dev/CodeGen'
import eventBus from '/@/utils/mitt'
import useClipboard from 'vue-clipboard3'

const { toClipboard } = useClipboard()
// 引入组件
const CodegenForm = defineAsyncComponent(() => import('./components/codegen-form.vue'))

const { proxy } = getCurrentInstance() as any

interface DbTree {
  key: string | null | undefined
  label: string | null | undefined
  type: string | null | undefined
  children?: DbTree[] | null
}
const codegenFormRef = ref()

const state = reactive({
  filter: {
    dbKey: '',
    dbType: '',
    config: {} as CodeGenGetOutput | null,
    dbTree: {} as DbTree,
  },
  // 基础数据定义

  defaultOption: {} as BaseDataGetOutput | any,
  // 界面显示用数据
  dbKeys: [] as Array<DatabaseGetOutput>,

  configs: [] as Array<CodeGenGetOutput>,

  dbTree: [] as Array<DbTree>,
  // 状态器定义
  dataLoading: false,
  dbStructShow: false,
  sels: [] as Array<CodeGenGetOutput>,
  importConfigs: [] as Array<CodeGenGetOutput>,
  importSels: [] as Array<CodeGenGetOutput>,
  importBoxyShow: false,
  importSuccess: false
})

const genDefaultConfig = (): CodeGenUpdateInput => {
  return {
    id: 0,
    authorName: state.defaultOption?.authorName ?? 'SirHQ',
    tableName: '',
    dbKey: state.filter.dbKey,
    entityName: '',
    comment: '',
    generateType: '1',
    apiAreaName: state.defaultOption?.apiAreaName ?? 'admin',
    namespace: state.defaultOption?.namespace ?? 'SirHQ.CodeGen.Apps',
    menuAfterText: state.defaultOption?.menuAfterText ?? '',
    busName: '',
    baseEntity: 'EntityBase',
    backendOut: state.defaultOption?.backendOut ?? '',
    frontendOut: state.defaultOption?.frontendOut ?? '',

    genEntity: true,
    genRepository: true,
    genService: true,

    genAdd: true,
    genUpdate: true,
    genDelete: true,

    fields: [],
  }
}
const getBaseData = async () => {
  state.dataLoading = true
  const res = await new CodeGenApi().getBaseData()
  state.dbKeys = res?.data?.databases ?? []
  delete res?.data?.databases
  state.defaultOption = res?.data
  state.dataLoading = false
}
const getTables = async () => {
  if (!state.filter.dbKey) {
    proxy.$modal.msgWarning('请选择数据源')
    return
  }
  const res = await new CodeGenApi().getTables({ dbkey: state.filter.dbKey })
  state.dbStructShow = true

  let dbTree: DbTree[] = []

  res?.data?.forEach((config: CodeGenGetOutput) => {
    let tree: DbTree = Object.assign({
      key: config.tableName,
      label: config.tableName + ' ' + config.comment,
      type: 'table',
      children: [],
    }, config)

    config.fields?.forEach((col: CodeGenFieldGetOutput) => {
      tree.children?.push({
        key: col.columnName,
        label: col.columnName + (' [' + col.dbType + (col.length != '-1' ? '(' + col.length + ')' : '') + ']'),
        type: 'col',
      })
    })

    dbTree.push(tree)
  })
  state.dbTree = dbTree
}
const getConfigs = async () => {
  if (!state.filter.dbKey)
    return
  state.filter.config = null
  state.dataLoading = true
  const res = await new CodeGenApi().getList({ dbkey: state.filter.dbKey })
  state.configs = res?.data ?? []
  state.dataLoading = false
}

const configSelect = async (row: CodeGenGetOutput, column: any, event: any) => {
  state.filter.config = row
}

const showEditData = async (data: CodeGenGetOutput) => {
  codegenFormRef?.value?.open(data)
}

const createTable = async () => {
  if (!state.filter.dbKey) {
    proxy.$modal.msgWarning('请选择数据源')
    return
  }
  showEditData(genDefaultConfig())
}
const removeConfig = async (config: CodeGenGetOutput) => {
  if (!config) {
    proxy.$modal.msgWarning('请选择表。')
    return
  }
  proxy.$modal.confirmDelete(`确定要删除【${config.tableName}】？`).then(async () => {
    var res = await new CodeGenApi().delete({ id: config.id }, { loading: true, showSuccessMessage: true })
    if (res?.success) {
      getConfigs()
    }
  })
}

const modifyConfig = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }
  showEditData(JSON.parse(JSON.stringify(config)))
}

const generate = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  await new CodeGenApi().generate({ id: config.id }, { loading: true, showSuccessMessage: true })
}

const compile = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  const res = await new CodeGenApi().compile({ id: config.id }, { loading: true, showSuccessMessage: false })
  console.log(res.data)
  if (res.data) {
    toClipboard(res.data)
    proxy.$modal.msgWarning('已将sql复制到剪切板&' + config.backendOut)
  } else {
    proxy.$modal.msgWarning('无迁移SQL')
  }
}

const genCompile = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  await new CodeGenApi().genCompile({ id: config.id }, { loading: true, showSuccessMessage: true })
}

const genMenu = async (config: CodeGenGetOutput) => {
  if (!config) return
  await new CodeGenApi().genMenu({ id: config.id }, { loading: true, showSuccessMessage: true })
}
// 确定
const onConfigEditSure = async (data: any) => {
  if (!data?.data) return

  let editorForm = data.data as CodeGenUpdateInput
  editorForm.dbKey = state.filter.dbKey
  if (!editorForm.fields) {
    editorForm.fields = []
  }
  // for (let idx in editorForm.columns) {
  //   editorForm.columns[idx].dbKey = state.filter.dbKey
  // }
  const res = await new CodeGenApi().update(editorForm, { loading: true, showSuccessMessage: true })
  if (!res?.success) {
    return res
  }
  state.filter.config = JSON.parse(JSON.stringify(editorForm))

  let callback = data.callback as Function
  callback(res)
  getConfigs()
}

const tableNodeSelect = async (obj: DbTree, node: any, prop: any, event: any) => {
  state.filter.dbTree = obj
}

const createConfigFromTable = async (table: DbTree) => {
  if (!table) return
  state.dbStructShow = false
  var newConfig = Object.assign(JSON.parse(JSON.stringify(table)), state.defaultOption)
  var baseFields = ['CreatedUserId', 'CreatedUserName', 'CreatedUserRealName', 'CreatedTime', 'ModifiedUserId', 'ModifiedUserName', 'ModifiedUserRealName', 'ModifiedTime', 'IsDeleted', 'Id']

  newConfig.fields = newConfig.fields.filter((item: any) => {
    return baseFields.indexOf(item.columnName) < 0
  })

  newConfig.fields.forEach((f: CodeGenFieldGetOutput) => {
    f.columnRawName = f.columnName
  })
  // newConfig.fields.reverse()
  newConfig.baseEntity = 'EntityBase'
  newConfig.generateType = '2'
  showEditData(newConfig)
}

const genType = (t: number) => {
  if (t == 1) return 'CodeFirst'
  if (t == 2) return 'DbFirst'
  return t
}
onMounted(() => {
  getBaseData()

  eventBus.on('onConfigEditSure', async (config: CodeGenUpdateInput) => {
    onConfigEditSure(config)
  })
})

onUnmounted(() => {
  eventBus.off('onConfigEditSure')
})

const selsChange = (vals: CodeGenGetOutput[]) => {
  console.log(vals)
  state.sels = vals
}
const exportConfig = () => {
  if (state.sels.length == 0)
    return proxy.$modal.msgWarning('请选择要复制的配置')
  console.log(JSON.stringify(state.sels))
  toClipboard(JSON.stringify(state.sels))
  return proxy.$modal.msgSuccess('已经配置复制到剪切板，保存成json即可')
}
const importDialogTableRef = ref()
const importConfig = () => {
  if (!state.filter.dbKey)
    return proxy.$modal.msgError('请先选择导入的数据源')
  state.importSuccess = false
  navigator.clipboard
    .readText()
    .then((v) => {
      console.log("获取剪贴板成功：", v);
      let list = JSON.parse(v) as Array<CodeGenGetOutput>
      state.importBoxyShow = true
      state.importConfigs = list
    })
    .catch((v) => {
      proxy.$modal.msgError('请先复制正确的配置')
    });
}
const importSelsChange = (vals: CodeGenGetOutput[]) => {
  state.importSels = vals
}

const onSureImport = () => {
  if (!state.filter.dbKey)
    return proxy.$modal.msgError('请先选择导入的数据源')
  state.importSels.forEach(async data => {
    if (data.importStatus == '导入成功' || data.importStatus == '导入中')
      return
    let editorForm = data as CodeGenUpdateInput
    editorForm.dbKey = state.filter.dbKey
    editorForm.id = 0
    if (!editorForm.fields) {
      editorForm.fields = []
    }
    editorForm.fields.forEach(s => {
      s.id = 0
    })
    data.importStatus = '导入中'
    const res = await new CodeGenApi().update(editorForm, { loading: true, showErrorMessage: false }).catch((e) => {
      data.importStatus = '导入失败:' + e.message
    })
    if (res?.success) {
      data.importStatus = '导入成功'
    }
    state.importSuccess = state.importSels.filter(s2 => s2.importStatus === '导入成功').length == state.importSels.length
  })
}
const onCancelImport = () => {
  state.importBoxyShow = false
}

</script>

<script lang="ts">
import { defineComponent } from 'vue'
import { ElLoading } from 'element-plus'
import { forEach } from "lodash"

export default defineComponent({
  name: 'dev/codegen',
})
</script>