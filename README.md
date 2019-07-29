# 项目地址：
* [码云上](https://gitee.com/linyee/MLBot) 原则上会优先维护与及更新
* [github上](https://github.com/jiaguoxinzhi/MLBot)

# MLBot
高智能机器人 High intelligent robot，基于ML.NET Based on ML.NET

# 项目依赖项
主要依赖ML.NET 其它依赖都尽可能使用知名度高且比较通用的依赖 以上两项依赖都解决不了，在解决方案里另起子项目以解决问题。

<details>
## 分词器 MLBot.NLTK
>可以直接使用jieba.net的数据
>自带数据在App_Data目录，整个复制到应用所在目录即直接使用。无数据时，会自动创建目录及文件。
## 分词命令行工具 MLBot.NLTK.CLI
>MLBot.NLTK.CLI "很多读者指出了不朽凡人的不足，有些我认为说的是对的，有些我认为和我的想法不同。如果说不朽凡人结尾的不好，我同意。要说烂尾，我真的不同意。关于量劫带来宇宙涅化的结尾，我还是比较满意的。因为在这里有各种人性的显露，人性在生死存亡面前的脆弱。"
</details>

# 跨平台性
* 因为本人C/C++不是很精通，所以程序控制台直接依赖dotnet core的跨平台性。另外会另起一个asp.net core 网站项目用webapi来实现跨平台。
* 有强力C/C++人员加入后再考虑自实现跨平台性。

# 设计初衷
* 用于微信公众号的自动聊天功能、与及自动处理业务的功能。尽可能的通用与广泛性支持。
* 逐步向高并发（信息爆炸）长时唠（信息锁链）多专业 高IQ高EQ高AQ发展

# 授权
遵循  [MIT 授权](https://github.com/jiaguoxinzhi/MLBot/blob/master/LICENSE)

# 已知成熟Ai对话机器人
* 微软小冰
* 图灵机器人：（好像收费，自己找吧）
* [BotSharp](https://github.com/SciSharp/BotSharp)

# 词汇解释
* IQ：Intelligence Quotient，智力商数，简称智商。学习能力、实践能力。
* EQ：Emotional Quotient，指情绪商数，简称情商。沟通能力、协作能力、情绪控制能力。
* AQ：Adversity Quotient，逆境商数，简称逆商或挫商。主动进取、承压能力。
* ?Q：自身控制能力（运动能力、毛孔控制、心跳控制、呼吸控制、图辨、声辨、嗅辨、味辨、触感、电力监控、温度监控、陀罗仪、重力感应、加速度感应）。

# 当前开发阶段 
> √完成 ×中止 △有Bug …进行中 ▲有严重Bug ☆优先开发中 ■终止开发（基本是作废）
* √尝试自建分词项目
* …构思框架与将要延伸的方向
* …尝试添加一些常用RESTful api
