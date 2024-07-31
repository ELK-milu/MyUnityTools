本目录主要是代码集中营
1.Application，框架中处于变更的代码，可能针对不同的项目需要设计到的定制
2.Mono,定制，项目的一些定制代码
3.Plusbe，框架代码，具有升级可行性，升级需要向下兼容
4.PlusbeGameStatus，定制，针对较为复杂的逻辑界面切换使用状态控制，一般的界面跳转开发者根据个人习惯
5.PlusbeTest，测试，主要是一些常用功能的测试参考代码
6.PlusbeUI,定制，界面UI逻辑控制代码


开发者使用流程:
1.使用ApplicationManager进行程序初始化，在resources/ui文件夹下面找到预制体ApplicationManager，可以进行常规化初始化配置，在对应的类中进行定制
2.使用UIManager进行UI管理，在resources/ui文件夹下面找到预制体UIManager,UI管理核心
3.在编辑器菜单栏找到“PlusbeTool/UI编辑工具/创建UI”,创建UI脚本并创建相关预制体，预制体路径（resources/ui），代码路径（Scripts/PlusbeUI）
4.在相关代码中进行常规逻辑代码编写
5.UIManager基本方法：
打开某个窗体：UIManager.OpenUI<xxxWindow>();
打开某个窗体并关闭自己：UIManager.OpenUI<xxxWindow>(this);
关闭某个窗体：UIManager.CloseUIWindow<xxxWindow>();UIManager.CloseUIWindow(uiBase);
对于一些需要提前创建但不需要显示：UIManager.CreateUIWindowOne<xxxWindow>();
6.UIWindowBase基本介绍
OnInit/OnOpen/OnClose

开发框架合并基础要求：
1.该版本主要未内部测试版，个人根据实际情况可以申请进行代码合并，也积极鼓励大家进行代码贡献
2.对于引用第三方类库，进行测试，尽量进行挑选，需要进行讨论审批
3.当前引入第三方dll,DOTween,Newtonsoft.Json,AVProVideo


发布程序基础要求：
1.按照指定的文件夹发布，数据存放在Datas文件夹下面
2.遵循常规配置文件信息，如窗体大小及位置，网络端口，控制IP及端口
3.根据项目实际情况是否对多个项目进行统一开发管理


