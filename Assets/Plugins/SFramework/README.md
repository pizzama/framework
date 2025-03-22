# SFramework 游戏开发框架

SFramework是一个专为Unity游戏开发设计的轻量级、高效的架构框架，采用MVC设计模式并结合模块化Bundle概念，为游戏开发提供了灵活而强大的架构支持。

## 核心特性

- **模块化设计**：基于Bundle概念实现功能模块化，便于团队协作和代码维护
- **MVC架构**：清晰分离数据、视图和控制逻辑，降低代码耦合度
- **生命周期管理**：完整管理游戏对象和模块的生命周期，优化内存使用
- **消息系统**：基于发布-订阅模式的消息传递机制，简化模块间通信
- **工具集成**：提供丰富的工具类库，简化常见游戏开发任务

## 目录结构

```
SFramework/
│
├── Script/              # 核心脚本目录
│   ├── Game/            # 游戏核心功能
│   │   ├── Core/        # 框架核心类库
│   │   ├── Actor/       # 角色相关功能
│   │   ├── UI/          # UI界面相关
│   │   ├── Event/       # 事件系统
│   │   ├── Camera/      # 相机控制
│   │   ├── Map/         # 地图系统
│   │   └── ...
│   ├── Tools/           # 工具类
│   │   ├── Math/        # 数学计算工具
│   │   ├── Extension/   # 扩展方法
│   │   └── ...
│   ├── Utils/           # 实用工具
│   └── Collections/     # 集合类
│
├── Editor/              # 编辑器扩展
│
└── Res/                 # 资源文件
```

## 核心类库

### 接口定义

- **ISBundle**：Bundle模块接口，定义了模块的生命周期和基本行为
  ```csharp
  public interface ISBundle
  {
      void Install();     // 安装模块
      void Uninstall();   // 卸载模块
      void Open();        // 打开模块
      void Close();       // 关闭模块
      void Update();      // 更新模块
      // ...其他方法
  }
  ```

- **ISManager**：管理器接口，定义了管理类通用行为

### 基础类

- **SBundle**：Bundle基类，实现ISBundle接口，提供模块基础功能
- **SBundleManager**：核心管理器，负责所有Bundle的生命周期管理和消息分发
- **SBundleParams**：参数封装类，用于消息传递和操作参数封装

### MVC架构组件

- **SControl**：控制器类，处理业务逻辑和消息调度
- **SModel**：模型类，负责数据处理和业务规则
- **SView**：视图类，负责UI界面显示和用户交互

### 辅助工具类

- **SEntity**：实体基类，提供基础实体能力
- **SMemory**：高效的双键索引数据结构，用于Bundle管理
- **SSingleton**：单例模式实现，提供全局访问点

## 架构设计理念

SFramework采用了多种设计模式和架构思想，主要包括：

### 模块化设计

每个Bundle代表游戏中的一个独立功能模块，具有完整的生命周期：
1. **安装(Install)**：模块初始化，注册到系统
2. **打开(Open)**：激活模块，开始工作
3. **更新(Update)**：每帧更新逻辑
4. **关闭(Close)**：暂时停用模块
5. **卸载(Uninstall)**：完全移除模块，释放资源

### MVC模式实现

- **控制器(SControl)**：负责业务逻辑，连接模型和视图
- **模型(SModel)**：管理数据和状态，封装核心算法
- **视图(SView)**：处理界面渲染和用户输入

### 消息系统

- 基于发布-订阅模式实现组件间解耦通信
- `SBundleParams`类封装消息内容和回调
- 支持模块间异步通信和序列操作

## 使用示例

### 创建一个基本模块

```csharp
// 每个游戏模块都要对应一组实现
public class GameContro : SControl
{
    // 在安装时执行
    public override void Install()
    {
        base.Install();
        Debug.Log("GameplayModule installed");
    }

    // 在打开时执行
    public override void Open(SBundleParams value)
    {
        base.Open(value);
        Debug.Log("GameplayModule opened");
    }

    // 在关闭时执行
    public override void Close()
    {
        base.Close();
        Debug.Log("GameplayModule closed");
    }
}
```

### 安装和使用模块

```csharp
// 安装模块
var gameplayModule = new GameplayModule();
SBundleManager.Instance.InstallBundle(gameplayModule);

// 打开模块
SBundleManager.Instance.OpenControl(typeof(GameplayModule).FullName);

// 关闭模块
SBundleManager.Instance.CloseControl(typeof(GameplayModule).FullName);

// 卸载模块
SBundleManager.Instance.UninstallBundle(gameplayModule);
```

### 模块间通信

```csharp
// 发送消息
var param = new SBundleParams();
param.MessageId = "SCORE_CHANGED";
param.MessageData = 100;
SBundleManager.Instance.AddMessageParams(param);

// 订阅消息
SBundleManager.Instance.SubscribeBundleMessage("SCORE_CHANGED", this);

// 处理消息
public override void HandleMessage(SBundleParams value)
{
    if (value.MessageId == "SCORE_CHANGED")
    {
        int score = (int)value.MessageData;
        Debug.Log($"Score changed: {score}");
    }
}
```

## 最佳实践

1. **明确职责划分**：
   - 控制器处理业务逻辑
   - 模型管理数据和状态
   - 视图负责界面展示

2. **合理拆分模块**：
   - 按功能划分Bundle
   - 保持单一职责
   - 避免模块间直接引用

3. **优化性能**：
   - 合理使用Update方法
   - 及时关闭和卸载不需要的模块
   - 避免大量消息传递

4. **资源管理**：
   - 在模块安装时加载资源
   - 在模块卸载时释放资源
   - 合理使用对象池

## 总结

SFramework提供了一套完整的游戏架构解决方案，具有以下优势：

- **高度模块化**：便于团队协作和功能扩展
- **低耦合设计**：各组件职责明确，降低依赖
- **强大的消息系统**：简化模块间通信
- **完整的生命周期管理**：自动化资源管理和内存优化
- **丰富的工具支持**：提高开发效率

通过采用SFramework，开发团队可以构建出结构清晰、易于维护和扩展的游戏项目。
