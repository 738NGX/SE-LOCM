# “缉古算今”游戏项目设计说明书
## 1. 简介

### 1.1 项目背景及作品创意

#### 1.1.1 项目背景

本作品——缉古算今（Stagnated Epoch:Legacy of Chinese Mathematics）是基于当下国家高度重视中国古代数学，以及中国古代数字对于当下数学领域以及科学领域重要的借鉴和指导意义的背景下而开创。本作品的游戏背景是科技高度发展的未来，游戏中"唤醒未来人类的思考能力、激发对中国古代数学的热爱同时也是在暗指希望“缉古算今”这款游戏可以激发玩家对中国传统古代数学的热爱，发现数学的美。本作品同时兼具关于中国古代数学内容的教育功能和基于闯关游戏的娱乐功能，作品结尾也将通过相应的设计引发用户关于当下科技时代如何对待传统经典智慧的反思，最后达到创新性继承和弘扬中华优秀传统文化，树立文化自信，挣脱科技“茧房”的功能效果。

#### 1.1.2 作品创意

本作品将《九章算术》作为核心主题，人物、剧情、地图、关卡等设计皆通过对该典籍的剖析进行创作，同时在用户闯关过程中利用细节潜移默化地宣传中国传统数学文化；与此同时，本作品结合当下高度发展的物理网、人工智能、元宇宙，构想未来发达的科技下诞生的九章世界，创作了一条现代与古代交织的游戏主线，设计了能够反映当代社会人性态度的故事情节。在作品的结尾部分用户将进一步深刻理解该作品的主体和内涵，对当下社会文化的态度进行反思反省。玩家既是现实世界的“玩家”，也是“缉古算今”中九章世界的玩家，具有更高的代入感和可玩性。

本作品在开发过程中，不只着眼于该游戏的教育意义，我们同样注重其作为“游戏”的可玩性。所以我们决定将其开发为肉鸽类（Roguelike）剧情冒险向卡牌游戏，游戏过程中具有相当程度的自由度，不同的玩家对NPC给出的选项产生不同的选择、以及对地图的不同的探索程度，都会导致游戏内容、游玩时长产生差异。本作品通过对《九章算术》这一中国数学典籍的剖析挖掘，设立与其相对应的九个章节的关卡，每一关卡中有4至6个剧情小节对应《九章算术》中相应章节的数学内容，玩家需要学习相关的数学知识，并通过具体的测试才能获得通关的机会。“九章”世界的剧情经过了本组成员长时间的打磨，它不再只是起到一个无聊的串联作用，而是真正的有血有肉，每一个人物都有自己鲜活的情感和饱满的人物形象，结尾也采用了开放性的结局，给到玩家更多自己的思考空间。

本作品的创意点还体现在美术设计上，本款游戏的原创美术素材采用了手绘与人工智能生成辅助绘图相结合的方式，既保证了高度原创性的同时，融入了当下先进的技术手段，使其游戏整体的配图自然流畅。

### 1.2 项目实施计划

【阶段性描述】

## 2. 多媒体系统整体设计

### 2.1 作品功能

#### 2.1.1 功能概述

本作品所实现的主要系统功能包括：地图系统、剧情系统、战斗系统、奖励系统、存档系统和图鉴系统。系统框架如下图所示：

![](./assets/功能概述.png)

图中黑色实线代表的是作品各个系统功能之间的从属关系；灰色虚线箭头代表的是游戏数据在各个系统功能之间的流向。每个主要系统实现的功能概述如下：

- 地图系统：串联起游戏各个系统的重要组成部分；
- 剧情系统：总计包含51段剧情（总时长约2-3小时），在推进主线剧情的同时向玩家传授各种与中国古代数学相关的知识，并且设置了可交互部分，以达到寓教于乐的效果；
- 战斗系统：和主线剧情相关联，使用代表数学知识的卡牌战胜敌人，赢取丰厚的奖励；
- 奖励系统：结算游戏中获得各种属性增益或者资源奖励；
- 存档系统：存储游戏状态到本地，以备之后再次读取调用；
- 图鉴系统：可以查询与游戏相关的各项信息，同时也包含了一些关于中国古代数学知识的介绍；

#### 2.1.2 功能说明与游戏机制

由于图鉴系统主要起到查询游戏相关信息的功能，因此将穿插在其他系统功能说明中一并介绍，不再作单独说明；存档系统并非玩家可以直接交互的功能，因此在此章节也暂不展开介绍。

##### 地图系统

本作品尽可能按照肉鸽类（Roguelike）游戏的设计模式进行设计，因此地图系统具有生成随机性、进程单向性、不可挽回性、游戏非线性等特点。玩家能够交互的地图界面如下图所示：

![](./assets/地图.png)

整张地图之间的路径均单向连接。玩家可以在一些节点自由选择进入哪条分支，但是不能走回头路。在地图中玩家已经通过的节点会被标记为红色，当前可以到达的节点标记为绿色，剩余不能到达的节点标记为灰色。一旦玩家在探险过程中死亡（体力值为0），只能从头开始探险。地图中可供玩家选择的地图节点类型说明如下：

- 剧情节点：玩家在剧情中与NPC互动学习数学知识，在剧情系统章节将会继续展开介绍；
- 战斗节点（包含敌人、精英、Boss）：玩家与遭遇到的各种敌人进行战斗并且获得奖励，在战斗系统章节将会继续展开介绍；
- 客栈节点：玩家可以在这里进入客栈，选择回复在战斗中失去的体力值、升级卡牌、获得银币或典籍、获得属性提升等效果；
- 商铺节点：玩家可以在这里使用获得的银币购买卡牌和典籍（有关典籍将会在典籍系统章节介绍），也可以在商铺用银币回收不需要的卡牌；
- 宝箱节点：玩家可以直接从宝箱节点获取银币和典籍奖励；
- 未知节点：玩家将会在这些节点遇到一些中国古代数学的谜题。如果回答正确，将会获得一种随机的正面增益；如果回答错误，同样会随机获得一种负面效果；

##### 剧情系统

玩家在剧情中与九章世界的原住民相识、互动、答题。不断学习和精进数学知识，解锁新的卡牌加入到卡池，并且获得各种奖励。玩家将会在剧情中逐渐结识九位原住民友人，她们将会在战斗中提供强大的助战效果。以下是玩家可以交互的剧情运行界面，包含了正常的文本对话模式和答题交互模式：

![](./assets/剧情1.png)

![](./assets/剧情2.png)

![](./assets/剧情3.png)

##### 战斗系统

在具体介绍战斗系统的功能及交互流程之前，需要先介绍其中几个重要的组成子系统：

**卡牌系统**

在游戏中，玩家获得的魔法能力以卡牌的形式体现。卡牌通过在剧情中与NPC互动学习数学知识被解锁并加入到卡池中，每当玩家获得卡牌奖励时，将会从卡牌池中随机选出默认三张不同卡牌供玩家选择一张加入到牌库中。卡牌可以用于在九章世界中和各类敌人进行战斗。卡牌奖励通常会在每场战斗结束后获得，剧情中也有可能会获得卡牌奖励。游戏中实装的不同卡牌数量为89张。

每一张卡面分别由以下几个部分组成（可参考示意图）：

- 卡牌名称：一张卡牌的标识名称，和中国古代数学相关。
- 卡牌效果：这张牌在战斗时被打出时的效果。在探险过程中玩家可以升级卡牌，从而获得更加强大的效果。
- 卡牌消耗：在战斗中玩家打出手牌需要消耗算术值（会在后文再介绍）。有些卡牌可以通过升级降低所需要消耗的算术值。
- 卡牌原文：与这张卡牌相关的典籍原文。（在图鉴页面，单击卡面上的原文可以触发全屏预览，方便玩家在游戏之余可以了解到卡牌的出处）。
- 卡牌类型：目前阶段已经实装的卡牌类型有三种：攻击牌、锦囊牌、装备牌。攻击牌（红色卡面）主要用于对敌人造成伤害。锦囊牌（蓝色卡面）主要在战斗中实现诸如增加护盾、额外抽牌弃牌等操作。装备牌（绿色卡面）可以给予玩家一些永久的数值增益或者能力。

![](.\assets\卡牌图鉴.png)

**典籍系统**

典籍是“九章”世界的文化瑰宝。每一本典籍都可以在探险和战斗中提供强大的增益效果。典籍的获得方式主要依靠地图中的宝箱节点来获得。在与敌人战斗结束后也有概率掉落典籍。在击败每一层的boss后必定掉落该层地图对应的九章算术残卷。集齐九章残卷后可以合成典籍九章算术。游戏中实装的不同典籍数量为35本。

![](./assets/典籍图鉴.png)

**战斗界面与流程**

![](./assets/战斗.png)

整个战斗界面如上图所示，界面上各个组成元素分别为：

- 顶部HUD：可以查看玩家当前的体力值、银币、获得的典籍、当前回合数等信息。也可以召唤友人进行助战（初始只有一名友人，随着主线剧情的推进会逐步解锁至9人，每使用1次友人助战，5次战斗后才能再次使用该友人进行助战）。
- 玩家与敌人信息：可以查看玩家与敌人的体力值、护盾值、基础攻击（防御）力、攻击（防御）倍率、增益效果等信息。敌人信息还包括敌人每回合的行动意图及意图数值（攻击、连击、重击、防御、回复血量、给予自身正面增益、给予玩家负面增益、攻击的同时组合另一种非攻击意图）。
- 算术值（左下角绿色龟壳上的左值）与初始算术值（右值）：玩家打出手牌需要消耗一定的算术值。在没有典籍加持下，每回合开始时玩家的算术值均为初始算术值。
- 抽牌堆（左下角黄色牌堆）、手牌（下方正中间）、弃牌堆（右下角蓝色牌堆）、消耗牌堆（右下角灰色数字中）：不同功能的牌堆。手牌堆直接显示并且可以打出，其他牌堆可以单击数字唤起全屏查看界面。
- 回合结束按钮（右下角卷轴）：结束当前回合玩家操作的阶段。

战斗采用回合制，一个回合分为准备阶段-抽牌阶段-玩家阶段-弃牌阶段-敌人阶段-结束阶段共六个阶段：

- 准备阶段：玩家的算术值回复为初始算术值。敌人生成本回合的行动意图（攻击、防御、回血等）并展示在敌人的血条上方。
- 抽牌阶段：从玩家的抽牌堆默认摸出5张牌加入到手牌中。
- 玩家阶段：玩家消耗算术值打出手牌堆中的手牌来造成各种战斗效果，打出的手牌会被移入到弃牌堆中（装备牌和个别有特殊说明的其他手牌移入到消耗牌堆中）。当玩家认为是时候结束回合时，可以点右下角的按键结束回合。由于每次抽牌时抽牌堆剩余卡牌数量不足时会自动将弃牌堆洗牌后重新放入抽牌堆。因此被移入消耗牌堆的卡牌不能在战斗中再被打出。
- 弃牌阶段：玩家剩余的手牌全部移入弃牌堆（个别有特殊说明的手牌移入到消耗堆中）。敌人失去之前获得的剩余护盾。
- 敌人阶段：敌人根据在准备阶段产生的意图进行行动。
- 结束阶段：清空玩家在本回合中的护盾，进入到下一回合。

当存活敌人数量为0时，战斗胜利，进入奖励页面；玩家体力值归零时，游戏失败。

##### 奖励系统

由于玩家在交互层面无法直接接触到底层的存档系统，于是作为玩家与底层数据之间的桥梁——奖励系统应运而生。奖励系统是与存档系统联系最为紧密的系统，负责收集并计算玩家在游戏过程中获得的各种奖励资源数据，并将其写入到游戏存档中。在不同情形下，玩家可以通过不同的方式与奖励系统交互：

**奖励页面**

奖励页面出现在每一次剧情和战斗的结束之后，地图上的宝箱节点也会触发奖励页面。奖励页面可以给予玩家银币奖励、若干次卡牌奖励、以及一定概率的典籍奖励。银币和卡牌奖励的数量随着战斗难度和剧情内容的不同而变化；在宝箱节点中没有卡牌奖励，但是一定能获得一本典籍。每一次卡牌奖励玩家可以从三张随机不同卡牌中选择一张加入到自己的牌组中。奖励页面的界面UI图如下所示：

![](./assets/奖励1.png)

![](./assets/奖励2.png)

**答题页面**

当玩家进入地图上的未知节点时会触发此一交互界面。玩家需要回答一道与中国古代数学相关的谜题，并且根据答案的正确与否，将奖励或者惩罚写入到存档数据中。答题页面的界面UI图如下所示：

![](./assets/未知.png)

**客栈页面**

当玩家进入地图上的客栈节点时会触发此一交互界面，玩家可以自由选择一间进入的房间，并且获得对应的奖励。客栈页面的界面UI图如下所示：

![](./assets/客栈.png)

**商铺页面**

当玩家进入到地图上的商铺节点时会触发此一交互页面。玩家可以通过使用在其他奖励页面获得的银币奖励与商人进行交易，来获得新的卡牌、典籍，或者是回收已经不需要的卡牌。商店页面的界面UI如下图所示：

![](./assets/商店.png)

### 2.2 软硬件运行平台

成品最后的编译生成版本为exe文件，仅支持Windows操作系统使用。根据Unity官方文档，使用Version2022.3编译的桌面端软硬件运行平台需求如下：

| 操作系统     | Windows                                       |
| ------------ | --------------------------------------------- |
| 操作系统版本 | Windows 7 (SP1+)， Windows 10 and Windows 11  |
| CPU          | x86、x64 架构（支持 SSE2 指令集）。           |
| 图形API      | 兼容 DX10、DX11、DX12。                       |
| 其他要求     | Hardware vendor officially supported drivers. |

额外说明的是，建议使用16:9比例的显示器运行作品，否则在全屏模式下可能会出现画面显示不全的问题（窗口模式则不受影响）。

### 2.3 系统开发平台

#### 2.3.1 素材创作

本作品中主要由作者创作的素材为图片素材。按照创作途径可以如下划分：

- 自主手绘设计：使用Adobe Photoshop CC 2021与Procreate；
- AI辅助生成：使用OpenAI ChatGPT 4.0 & DALL-E 3；

#### 2.3.2 交互与代码设计

本作品在交互设计中使用的游戏引擎为Unity，版本号为：

- Unity Hub V3.3.1-c7；
- Unity Editor Version2022.3.15f1c1；

本作品在交互设计中使用的代码编辑器/集成开发环境包括：

- Microsoft Visual Studio Code 1.87.0；
- Microsoft Visual Studio 2022 Community 17.9.0；
- JetBrains Rider 2024.1；

本作品在交互设计中使用的版本控制工具为：

- 本地：git version 2.42.0；
- 远程：Github 私有仓库；

#### 2.3.3 开源平台及第三方工具

本作品中主要使用的开源平台及第三方工具均为Unity插件/第三方库，来源为Github或Unity官方插件商铺，均为免费资源。具体如下：

- [Demigiant/dotween(github.com)](https://github.com/Demigiant/dotween)：界面动画视效插件；
- [snozbot/fungus(github.com)](https://github.com/snozbot/fungus)：剧情交互插件；
- [Warrior Free Asset | 2D 角色 | Unity Asset Store](https://assetstore.unity.com/packages/2d/characters/warrior-free-asset-195707)：战斗界面的玩家模型；
- [Monsters_Creatures_Fantasy | 2D 角色 | Unity Asset Store](https://assetstore.unity.com/packages/2d/characters/monsters-creatures-fantasy-167949)：战斗界面的敌人模型；
- [Fantasy Wooden GUI : Free | 2D GUI | Unity Asset Store](https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811)：GUI界面素材；
- [Pixel Art Icon Pack - RPG | 2D 图标 | Unity Asset Store](https://assetstore.unity.com/packages/2d/gui/icons/pixel-art-icon-pack-rpg-158343)：GUI界面素材；

### 2.4 作品特色

【重点介绍本作品在创意、制作、开发实现、应用等方面的亮点，有特色的功能，团队重点解决的问题等。】

## 3 多媒体系统详细设计

### 3.1 构想（创意）

本作品的构想是，在一个科技飞速发展的遥远未来，人类社会迎来了转抉性的变革。高级人工智能和机器人彻底接管了生产与服务行业，释放了人类从学习到工作的全部负担。人类的智慧被先进的自然语言模型所解放，进入了被称为“停滞纪元”的新时代。在这个时代，人类创造了史上最伟大的发明——“新世界”，这是一种结合了人工智能、物联网及元宇宙技术的虚拟现实设备。人们通过它进入一个庞大而细腻的虚拟世界，体验极致逼真的感官和情感。然而，这种依赖让人类渐渐丧失了思考能力。在这个由钢铁建筑覆盖、环境破坏严重的现世中，一切似乎都停滞了。为了唤醒人类沉睡的思维，“新世界”公司开创了一个古代中国数学挑战区——“九章”，希望人们通过游戏的形式重燃对数学的热爱，激活沉睡的智慧。

### 3.2 剧情与角色设计

#### 3.2.1 剧情

有关本作品的编剧方面，九章世界的故事分为明暗两条线，明线是主人公也就是各个玩家一层一层的冒险，每一层对应《九章算术》一个章节，有着独立的故事情节和《九章算术》所涉及到的数学知识点，主人公试图探寻九章世界的真正秘密并且回到现实世界；暗线则是主人公九层所经历的一切，不过是“九章”底层逻辑“生命”与“毁灭”的一个赌约，指向的是“生命”不满人类将原本学习数学知识的“九章”变为盈利工具，并且所有玩家也并没有真正的去学习数学、热爱数学，进而导致生命的扭曲，试图要将九章世界变为现实，彻底吞并人类。同时，结尾为开放性结局，旨在留给玩家更多的思考和想象空间。

#### 3.2.2 角色

本作品中所涉及到的人物除主人公外分为两种，一种为Boss，一种为友人。友人负责指引玩家学习每一层对应的《九章算术》中的数学知识，并且推动故事情节的发展，构建玩家与九章世界的情感羁绊。

而Boss的设计也分为两种，“七宗罪”和生命毁灭。其中生命和毁灭同为九章世界底层逻辑的人格具象，而七宗罪则是生命扭曲了七位玩家内心的黑暗面所产生，用于完成其与毁灭之间的赌约。

值得注意的是，每一位角色都有着饱满丰富的形象，和自身独特的性格，并不是扁平人物的单纯的“好”或者单纯的“坏”。

| 角色名 | 形象设定                                                     |
| ------ | ------------------------------------------------------------ |
| 叶羽   | 在“九章”经营万事屋（占卜店）的少女。性格固执，性情古怪，但心地很善良，不太会拒绝他人的请求。 |
| 花菀   | 叶羽的儿时玩伴。喜欢好吃的东西，靠售卖粮食营生。             |
| 曜     | 在“九章”从事捕鱼的少女。总是很开朗、精力充沛，运动神经拔群。 |
| 陆彼   | 戴雅的妹妹，在官府担任书吏。有些内向。                       |
| 郭南   | “九章”的著名工匠。很多宏伟的大工程均出自她的手笔。           |
| 戴雅   | “九章”的地方官员。聪明美丽，绝不允许歪风邪气的完美主义者。   |
| 千歌   | “九章”历史悠久的客栈的老板娘三姐妹中的老幺。不怕生，讨厌失败。 |
| 黎紫   | 孜孜求学的学者。性格平和，但充满好奇心，有着自命不凡的一面。 |
| 鞠笠   | 岛上城堡里居住的领主末裔。不怎么在人前展露身姿。             |
| 傲慢   | 被生命放大内心缺点＂傲慢＂ 的玩家，极为的心高气傲，看不上叶羽的占卜之术，进而被生命所利用来完成与毁灭的赌约。 |
| 暴食   | 原本为一名热爱美食的玩家，但因自身身材瘦弱，食量很小，进而产生了心理扭曲，被生命所利用化身为“暴食”。 |
| 嫉妒   | 一个能力普通的玩家，现实世界和游戏世界的生活都极其的平庸，所以十分嫉妒天资聪颖或者非常优秀的人，进而被生命所利用，化身为“嫉妒”。 |
| 懒惰   | 原本是一位大智若愚的玩家，表面很懒惰、非常懒散，其实是用最少的成本得到最大的利益，不去做无谓功。被生命强制放大懒惰的一面，化身为“懒惰”，后期摆脱了生命的控制，诞生自己的意识。 |
| 暴怒   | 一个武痴玩家，性格刚正但急躁，被毁灭所不喜，被生命扭曲为暴怒，后与懒惰一样恢复自己的意志。 |
| 贪婪   | 一名贪婪成性的玩家，被生命放大内心的贪欲化身“贪婪”，后作为祭司控制戴家。 |
| 色欲   | 一名极端的女权主义者，极为美丽，喜爱运用自己的美貌坑骗男性玩家，被生命控制化身“色欲”，将盈不足世界变为女权社会。 |
| 毁灭   | 与生命同为九章世界底层逻辑人格，代表的是暴虐毁灭的一面，想要彻底毁灭人类，由人工智能控制地球，因意见与生命不合与其进行了赌约。 |
| 生命   | 九章世界底层逻辑的主人格，试图将九章世界由虚拟变为现实，原本企图唤醒人们对数学的喜爱，激活人们的思考能力，但后来对人类失望进而试图“净化”人类，九章世界爆炸后不知下落…… |

### 3.3 艺术设计

本作品在艺术设计方面试图展现独特的创意和较为出色的技术运用。游戏的整体画风为中国古代风格，这种风格的选择不仅符合游戏的主题和背景，也为玩家营造了一种古朴高雅的游戏氛围。在游戏中，玩家可以感受到浓郁的中国文化气息，仿佛置身于一个充满历史底蕴的古代世界。

除了部分UI设计来自Unity官方商店的免费素材，本作品中大部分的美术素材均为原创通过手绘与人工智能生成辅助绘图相结合的方式，既展现了高度的原创性，又融入了先进的技术手段，使得游戏美术配图既自然又流畅。手绘的细腻和独特性是任何技术都无法替代的。在游戏中，手绘的人物立绘无疑是一大亮点，每一个细节都经过精心描绘，无论是人物的表情、服饰还是动作，都栩栩如生，仿佛跃然纸上。这种细腻的手绘风格，不仅为游戏增添了独特的艺术气息，也为玩家带来了更加舒适的视觉感受。而人工智能生成辅助绘图技术的运用，则使得游戏在保持手绘风格的同时，又具备了更高的制作效率和更丰富的表现形式。各种风格的配图，使得游戏的整体视觉效果更加出色。

本作品的声效来源于网络素材，采取的是中西结合、古今结合的方式，根据不同内容进行针对性的配乐，因游戏自身的包容性，玩家在游玩中会感受到各国、各个时间音乐的交融，照应“缉古算今”的主题。

### 3.4 程序系统设计与编程

#### 3.4.1 功能模块设计

在多媒体系统整体设计章节提到本作品所实现的主要系统功能包括地图系统、剧情系统、战斗系统、奖励系统、存档系统和图鉴系统。接下来将逐个对系统的各个功能模块设计进行介绍：

##### 地图系统、剧情系统、奖励系统与存档系统

```mermaid
classDiagram
MonoBehaviour<|--SceneFader
MonoBehaviour<|--StorySceneLoader
MonoBehaviour<|--SkipStory
MonoBehaviour<|--Map
MonoBehaviour<|--MapNode
MonoBehaviour<|--Reward
MonoBehaviour<|--RewardItem
MonoBehaviour<|--Problem
MonoBehaviour<|--Break
MonoBehaviour<|--Shop
MonoBehaviour<|--ShopItem
LocalSaveData<--LocalSaveDataManager
LocalSaveData<--Map
LocalSaveData<--StorySceneLoader
LocalSaveData<--Problem
LocalSaveData<--Break
LocalSaveData<--Reward
LocalSaveData<--Shop
StorySceneLoader<|..SkipStory
SceneFader<--Map
SceneFader<--MapNode
SceneFader<--StorySceneLoader
SceneFader<--Problem
SceneFader<--Break
SceneFader<--Reward
SceneFader<--Shop
Map*-->MapNode
Reward<--RewardItem
Shop<--ShopItem
ShopItem<|--CardInShop
ShopItem<|--DeleteCard
ShopItem<|--RandomBook
namespace 存档系统{ 
    class LocalSaveData{
        +LocalSaveStatus status
        +int hp
        +int hpLimit
        +int initAp
        +int initDp
        +int initSp
        +int coins
        +List cardsData
        +List<int> booksData
        +List<int> cardsPool
        +List<int> friends
        +List<int> route
        +int Level
        +FriendsCoolDown()
    }
    class LocalSaveDataManager{
        <<staic>>
        +SaveInitLocalData()
        +SaveLocalData()
        +LoadLocalData()
    }
}
class SceneFader{
    +Image fadeImage
    +float fadeDuration
    +FadeIn()
    +FadeOut()
}
namespace 剧情系统{
    class StorySceneLoader{
        +Prologue()
        +Transmission()
    }
    class SkipStory{
        -Button skipButton
        +TryStopFungusMusic()
    }
}
namespace 地图系统{
    class Map{
        +BackTheme()
    }
    class MapNode{
        +int id
        +MapNodeStauts status
        -LoadScene()
    }
}
namespace 奖励系统{
    class Reward{
        +RewardCoins()
        +WaitRewardCard()
        +ChangeSelectingRewardCard()
        +RewardCard()
        +SkipRewardCard()
        +RewardBook()
        +Continue()
    }
    class RewardItem{
        +RewardType type
        +AudioClip sfx
        +RewardClicked()
    }
    class Problem{
        +ProblemInfo UsingProblem
        -List<int> problemIds
        -List<int> seeds
        +CallTrueAnswer()
        +CallFalseAnswer()
        +CallEndProblem()
        -GetProblemDataId(int digit)
    }
    class Break{
        +ExecuteBreak()
        -Recover()
        -CallCardUpgrade()
        +ChangeSelectingCardIndex()
        +UpgradeCard()
        -AdjustCoins()
        -TryGetRewardBook()
        -AddAttack()
        -AddDefence()
        -WaitEndBreak()
        +EndBreak()
    }
    class Shop{
        +Purchase()
        -GetItemById()
        -ProcessPurchase()
        -GetCardGoodsFromPool()
        -CallCardDelete()
        +ChangeSelectingCardIndex()
        +DeleteCard()
        +EndShop()
    }
    class ShopItem{
        +int id
        +Shop shop
        +int Price
        +bool IsPurchased
        -int countRate
    }
    class CardInShop{
        +CardDisplay cardDisplay
        -bool inited
        +int InitId
        +GetCard()
        +UpdateCardGoodInfo()
    }
    class RandomBook{
        +GetBook()
    }
    class DeleteCard
}
```

这四个系统之间的功能依赖关系较为紧密，因此绘制在同一张模块调用关系图中，如上图所示。（为了避免繁杂，只展示类型中一些主要的类型及方法。下同）下面是对这四个系统中模块的功能介绍：

- `MonoBehavior`：Unity脚本的共同父类。（下同，不再重复介绍）
- `SceneFader`：场景跳转模块。提供方法在加载场景时淡入；或跳转到目标场景时淡出。
- `LocalSaveData`：游戏存档的储存模块，包含了游戏过程中玩家的各项属性、背包资源、解锁地图等数据；同时包含了一些便于对存档数据进行读写操作的方法。大部分系统依赖于该模块来交换和暂存数据。
- `LocalSaveDataManager`：静态模块，提供方法从本地文件读取存档；或将存档保存为本地文件。
- `StorySceneLoader`：提供方法在剧情结束后跳转到下一个目标场景。
- `SkipStory`：提供方法直接快速跳过剧情。
- `Map`：构成地图系统的主要模块。本身具有与存档系统交互的功能，同时提供方法使玩家返回主界面。
- `MapNode`：与`Map`模块组合，记录每一个地图节点的相关数据，提供方法跳转到该节点对应的场景。
- `Reward`：实现在宝箱节点或剧情/战斗结束之后向玩家提供各种奖励功能的模块，本身具有与存档系统交互的功能。
- `RewardItem`：依赖于`Reward`模块，调用`Reward`中的对应方法向存档中写入奖励数据。
- `Problem`：提供方法实现地图中未知节点中答题功能的模块，本身具有与存档系统交互的功能。
- `Break`：提供方法实现地图中客栈节点的模块，本身具有与存档系统交互的功能。
- `Shop`：提供方法实现地图中商铺节点的模块，本身具有与存档系统交互的功能。
- `ShopItem`：依赖于`Shop`模块，调用`Shop`中的对应方法从存档中读取剩余银币进行交易，并将结果数据写入到存档中。
- `CardInShop`：继承自`ShopItem`，主要实现从商铺中购买卡牌的功能。
- `DeleteCard`：继承自`ShopItem`，主要实现从商铺中回收卡牌的功能。
- `RandomBook`：继承自`ShopItem`，主要实现从商铺中抽选典籍的功能。

##### 战斗系统

```mermaid
classDiagram
CardList o-->Card
CardPile<|--DrawPile
CardPile<|--HandCards
MonoBehaviour<|--HandCardsUI
HandCards<--HandCardsUI
CardPile<|--DiscardPile
MonoBehaviour<|--BookViewer
GameController<--BookViewer
MonoBehaviour<|--CardPileViewer
GameController<--CardPileViewer
MonoBehaviour<|--BezierArrows
GameController<--BezierArrows
MonoBehaviour<|--RoundButton
GameController<--RoundButton
MonoBehaviour<|--BuffInfo
GameController<--BuffInfo
MonoBehaviour<|--CardTemplate
GameController<--CardTemplate
Card<--CardTemplate
HandCardsUI*-->CardTemplate
MonoBehaviour<|--CardPile
GameController<-->CardPile
CardPile*-->CardList
MonoBehaviour<|--FriendItem
GameController<--FriendItem
MonoBehaviour<|--GameController
MonoBehaviour<|--DisplayController
GameController*-->DisplayController
MonoBehaviour<|--Creature
GameController<-->Creature
MonoBehaviour<|--SavesController
GameController*-->SavesController
Creature*-->BuffContainer
BuffContainer o-->Buff
Creature<|--Enemy
Creature<|--Player
class GameController{
    +PlayAudio()
    -EnemyInit()
    +DrawCards()
    +WaitingDiscards()
    -SelectAttack()
    +AllAttack()
    +AddShield()
    +UpArrow()
    +DownArrow()
    -AttackPointsAdjust()
    -DefencePointsAdjust()
    +CardExecuteAction()
}
class DisplayController{
	+UpdateHPSlider()
    +AnimatePanelAndText()
    +CreatePanel()
    -CreateText()
}
class SavesController{
    +LocalSaveData localSaveData
    +LoadLocalData()
    +SaveLocalData()
}
class Creature{
    +int hp
    +int hpLimit
    +int ap
    +int dp
    +int shield
    +BuffContainer buffContainer
    -int roundCount
    +AddHP()
    +ReduceHP()
    +AddShield()
    +AddBuff()
}
class BuffContainer{
    +List<Buff> buffs
    +float AttackRate
    +float DefenceRate
    +int StealedCoins
    +int ExtraCard
    +int ExtraSP
    +BuffContainerStatus
    +AddBuff()
    +CallAttack()
    +CallDefence()
    +CallPlayCard()
    +EffectUpdate()
    +RoundUpdate()
    +Clarify()
    -RemoveBuff()
    -ExistBuff()
    +GetLevel()
}
class Buff{
    +int id
    +string name
    +BuffType Type
    +BuffStyle Style
    +string Effect
    +int Level
    +Buff()
    +ChangeLevel()
    +DecreaseLevel()
    +IncreaseLevel()
}
class Enemy{
    +bool active
    +int id
    +int index
    +IntendType intendType
    +int intendValue
    +int intendTimes
    -EnemyInfo info
    -Buff giveBuff
    -Buff extraGiveBuff
    -Creature buffReceiver
    -bool waitSelect
    +Init()
    +Prepare()
    -DecideGrowth()
    -GetIntendType()
    -GetIntendValue()
    +IsIntendAttack()
    +Execute()
    -Attack()
}
class Player{
    +int sp
    +int spInit
    +int coins
    +List<int> books
    +Init() 
    +RecoverSp()
    +AddSp()
    +ReduceSp()
    +AddCoins()
    +ReduceCoins()
    +ContainsBook()
}
class FriendItem{
    +int index
    +int WaitRound
    -DisplayInstruction()
    -CallFriend()
}
class CardList{
    -List<Card> Cards
    +int Count
    +int CountAttackCards
    +int CountSpellCards
    +int CountEquipCards
    +AddCards()
    +RemoveCards()
    +ContainsCard()
    +ClearCards()
    +UpgradeAllCards()
    +Top()
}
class Card{
    +int id
    +int cost
    +CardType type
    +CardRarity rarity
    +bool isPlused
    +bool disposable
    +int playTimes
    +CardDisplayInfo displayInfo
    +Play()
    +Upgrade()
    +Export()
    -ReadCardData()
}
class DrawPile{
    +Init()
    -Shuffle()
    +DrawCards()
}
class HandCards{
    +AddExtraCards()
    +DisposeNonAttackCards()
}
class HandCardsUI{
    -float cardWidth
    -float spacing    
    +Rect PlayArea
    +ReturnCards()
    +CardDisplayInfoUpdate()
    -CardDisplayIndexUpdate()
    +DrawCards()
    +DisposeNonAttackCards()
    +RemoveCard()
    +CardsDisplayInfoUpdate()
    +CardDisplayUpdate()
    +DisCards()
}
class DiscardPile{
    +List<Card> discards
    +List<Card> disposedCards
    +AddCardsToDiscard()
    +AddCardsToDisposable()
}
class BookViewer{
    -Init()
    +OpenPage()
    +ClosePage()
    +UpdateBookDisplayInfo()
    +DisplayInstruction()
}
class CardPileViewer{
    +OpenPage()
    +ClosePage()
}
class BuffInfo{
    +OpenPage()
    +ClosePage()
}
class CardTemplate{
    +Vector3 originalScale;
    +Vector3 originalPosition;
    +int index
    +bool inHand
    +bool isDragging
    +Card BindCard
    -bool inPlayArea
    -RemoveCard()
    -DisCard()
}
```

战斗系统的功能模块设计如上图所示，接下来对每个模块的功能进行介绍：

- `GameController`：整个战斗系统的核心控制模块，战斗系统中的基本所有模块都依赖于该模块运行。实现负责整场战斗中所有底层数据以及游戏进程的控制的功能。其封装的主要方法包括：

    - 播放音效
    - 初始化敌人
    - 抽牌、弃牌、攻击、防御、给予增益效果、调整攻击力等游戏控制功能
    - 每一种卡牌打出后的具体效果方法（通常是上一条中几种方法的组合）

- `DisplayController`：与`GameController`组合，实现负责控制整个战斗系统中的用户界面显示内容的功能。其主要封装的方法包括：

    - 在每一帧实时刷新用户页面上显示的各项数据
    - 如果玩家和敌人的血量发生变化，提供平滑的过渡动画
    - 弹出文字提示框

- `SavesController`：与`GameController`组合，实现负责控制战斗系统与存档系统之间的数据交互的功能。其主要封装的方法包括从本地文件读取存档并协助`GameController`完成初始化；以及在战斗结束后重新结算玩家血量等数据存储回本地文件。

- `Creature`：玩家和敌人的共同基类，包括了一些共同的属性（如血量、攻击力、防御力等）和方法。

- `Enemy`：继承自`Creature`类的敌人模块，除了重载基类部分方法外主要增加了有关回合意图的控制功能。

- `Player`：继承自`Creature`类的玩家模块，除了重载基类部分方法外主要增加了有关玩家算术值和典籍的相关属性和方法。

- `BuffContainer`：与`Creature`组合，实现记录、计算玩家或敌人当前身上的正负增益效果并影响玩家或敌人的各项属性值的功能。

- `Buff`：由`BuffContainer`聚合，代表每一种正负增益效果的相关信息。`BuffContainer`根据一个列表属性中包含的所有Buff来计算最后的增益效果总和。

- `FriendItem`：实现友人助战功能的模块。具有属性记录每一位友人的ID与冷却状态来判断是否可用，如果可用则提供链接到`GameController`的方法来触发友人助战。

- `CardPile`：游戏中卡牌堆的共同基类，本身只继承了`Monobehavior`的Unity脚本方法，但与`CardList`模块组合实现了牌堆的各项功能。

- `CardList`：本身为卡牌列表的扩展模块，封装了一个卡牌列表和一些便于对齐进行读写操作的方法。

- `Card`：由`CardList`聚合，记录卡牌各项属性的模块，同时也封装了一些方法支持快速对卡牌信息进行更新操作。

- `DrawPile`：继承自`CardPile`模块，实现了抽牌堆的功能。其封装的独有方法包括：

    - 从`SavesController`读取存档数据并初始化牌组
    - 洗牌（重置牌序，初始化后和弃牌堆卡牌返回抽牌堆时触发）
    - 摸牌（从牌堆中弹出参数传入数量的卡牌列表并返回）

- `DiscardPile`：继承自`CardPile`模块，实现了弃牌堆的功能。虽然名为弃牌堆，但是实际上同时管理着两个卡牌列表——弃牌堆和消耗牌堆。所以该模块封装了两组独立的方法以对两个牌堆进行读写控制。

- `HandCards`：继承自`CardPile`模块，实现了手牌堆的功能。其封装的独有方法大多是为了适配一些卡牌打出后对手牌牌组进行直接修改的情况，包括：

    - 立刻添加一个列表的额外卡牌到手牌堆中（主要是集成了刷新UI显示的方法，减少不必要的重复调用，下同）
    - 消耗手中所有卡牌类型不属于攻击牌的手牌

- `HandCardsUI`：由于手牌堆的UI显示机制较为复杂，于是将其独立于`DisplayController`之外单列控制。其封装的属性和方法包括：

    - 显示卡牌的宽度与间距
    - 手牌UI拖动后触发判定是否可以被打出的屏幕范围
    - 手牌堆中卡牌数量增减后平滑过渡的动画方法
    - 刷新手牌堆中所有卡牌的显示信息，时刻保持与手牌堆数据情况一致的方法

- `CardTemplate`：手牌堆中UI显示的卡牌模板，组合于`HandCardsUI`。手牌堆UI中所有显示的卡牌都由此模板复制而来。封装的类型记录了其原始的UI索引、绑定卡牌、位置缩放等信息；封装的方法实现了其可以被自由拖动并打出的功能，并将打出的信息广播给`GameController`进行调度。

- `BookViewer`：实现了支持玩家在战斗中查看已经获得的典籍及其效果的功能。

- `CardPileViewer`：实现了支持玩家在战斗中全屏预览抽牌堆、弃牌堆和消耗牌堆中卡牌内容功能。每个牌堆的UI绑定了一个不同的整型数作为参数传入，从而决定了在前台显示哪一个牌堆。

- `BuffInfo`：实现了支持玩家在战斗中查看自身或者敌人身上所有正负增益效果的功能。每个`Creature`的UI绑定了一个不同的整型数作为参数传入，从而决定了在前台显示玩家或是某一个具体敌人身上的正负增益效果。

- `BezierArrows`：针对某些卡牌打出后需要选中一个敌人触发效果，专门生成的视觉效果。当卡牌打出后直到选中敌人前，手牌堆UI到鼠标指针之间会出现一条由贝塞尔曲线计算的箭头。该模块主要实现这个视觉效果，如下所示：

    ![](./assets/贝塞尔.png)

- `RoundButton`：绑定在回合结束按钮上，实现链接到GameController上触发结束玩家回合事件的方法。

##### 图鉴系统

由于游戏主界面与图鉴界面之间的相似共性较多，因此这两个界面场景同属于图鉴系统。

```mermaid
classDiagram
MonoBehaviour<|--MainTheme
MonoBehaviour<|--ThemeButton
MainTheme*-->ThemeButton
MonoBehaviour<|--ThemePop
MainTheme*-->ThemePop
MonoBehaviour<|--Wiki
MainTheme*-->Wiki
Wiki<--WikiScrollView
WikiScrollView<|--CardSelector
WikiScrollView<|--BookSelector
WikiScrollView<|--CharacterSelector
WikiScrollView<|--StorySelector
MonoBehaviour<|--CardDisplay
Wiki*-->CardDisplay
MonoBehaviour<|--BookDisplay
Wiki*-->BookDisplay
MonoBehaviour<|--CharacterDisplay
Wiki*-->CharacterDisplay
MonoBehaviour<|--CardQuote
class MainTheme{
	+SceneFader sf
    +bool popWindow
    +PopWinodw()
    +DisPopWindow()
    +StartGame()
    +StartNewGame()
    +ContinueGame()
    +ExitGame()
    +NextScene()
    +PlayAudio()
}
class ThemeButton{
    +int id
    -Transform text
    -bool waitClick
}
class Wiki{
    +WikiStatus wikiStatus
    +ChangeWikiStatus()
    +ChangeWikiStatus()
    +UpdateCardDisplayInfo()
    +UpdateBookDisplayInfo()
    +UpdateCharacterDisplayInfo()
}
class WikiScrollView{
    +float itemHeight
    +float spacing
    +int itemCount
    -AdjustHeight()
    +PopulateList<T>()
}
class BookDisplay{
    +Image image
    +TextMeshProUGUI bookName
    +TextMeshProUGUI effect
    +TextMeshProUGUI introduction
    +UpdateBookDisplayInfo()
}
class CardDisplay{
    +int id
    +bool isPlused
    +int displayIndex
    -bool idSet
    +UpdateCardDisplayInfo()
    +UpgradeExchange()
}
class CharacterDisplay{
    +Image image
    +TextMeshProUGUI charName
    +TextMeshProUGUI introduction
    +UpdateCharacterDisplayInfo()
}
class CardQuote{
    -bool isClicked = false;
    -Vector3 originalPosition;
    -Vector3 originalScale;
    -Color originalTextColor;
    -ToggleState()
}
```

图鉴系统的模块设计如上图所示，接下来对每个模块的功能进行介绍：

- `MainTheme`：主要实现游戏主界面各项功能的模块。其封装的方法主要包括：
    - 弹出选择弹窗供玩家确认（是否通过已有存档继续游戏或开启新游戏；是否退出游戏）
    - 通过已有存档继续游戏、从初始存档开始新游戏
    - 退出游戏
    - 【静态方法】根据当前存档记录的最后一个地图节点位置判断下一个跳转的目标场景，整个项目都可调用该方法
    - 【静态方法】更简单地播放音效，整个项目都可调用该方法
- `ThemeButton`：与`MainTheme`组合，游戏主界面和图鉴界面共同使用的按钮模块，根据id属性的不同决定了其不同的点击后触发功能。
- `ThemePop`：与`MainTheme`组合，主要实现控制主界面的两种弹窗的功能。
- `Wiki`：主要实现图鉴页面各项功能的模块。因为共用了`ThemeButton`，因此与`MainTheme`组合发挥作用。
- `WikiScrollView`：实现图鉴中各个项目滚动选择列表的模板类，封装了初始化滚动选择列表的方法。继承该模块的子类则是分别为了适应不同的源数据类型（卡牌、典籍、角色、剧情）应运而生，因此不再作额外介绍。
- `CardDisplay`、`BookDisplay`、`CharacterDisplay`：分别实现图鉴页面显示卡牌、典籍、角色具体信息功能的模块。其封装了需要显示的信息属性以及更新当前显示内容的方法。
- `CardQuote`：主要实现卡牌图鉴页面卡牌原文单击能全屏预览的功能，其封装了文字组件的原始状态属性和一些平滑动画方法。

#### 3.4.2 数据结构设计



## 4. 系统安装及使用说明

解压编译完成的文件压缩包，直接运行`SELOCM/SELOCM.exe`即可。

## 5. 总结

本作品基于对中国古代数学著作《九章世界》的学习与剖析，结合对当下人们过于依赖科技产品的社会现状的反思，积极响应国家关于弘扬中华优秀传统文化的号召，创作了一条古今相交的游戏故事主线；在原创学习剧情的同时，还设计了诸多极具热血冒险风格的游戏情节。

在游戏素材方面，我们通过原创剧情故事、手绘人物形象、AI创作背景图片等方式，力求游戏画面独特美观；在作品可玩性方面，我们通过灵活的后台设置，使其游戏小节随机性大大提高，不同时间的游戏体验均会有所区别；在作品立意方面，我们围绕弘扬中国优秀传统文化、科学使用现代科技产品的主题展开项目，力求“寓教于乐”的功能效果。

综上所述，本作品是一项以弘扬中华优秀传统文化、科学利用现代科技产品为主题的，兼具娱乐性和教育性的寓教于乐的数媒设计作品。

## 6. 附录

制作过程中的素材目录清单

讨论议题、会议纪要、访谈记录、邮件等清单

名词定义

参考资料