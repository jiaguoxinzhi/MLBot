# 解决方案地址：
* [码云上](https://gitee.com/linyee/MLBot) 原则上会优先维护与及更新
* [github上](https://github.com/jiaguoxinzhi/MLBot)

# MLBot
高智能机器人 High intelligent robot，基于ML.NET Based on ML.NET

# 解决方案依赖项
主要依赖ML.NET 其它依赖都尽可能使用知名度高且比较通用的依赖 以上两项依赖都解决不了，在解决方案里另起子项目以解决问题。

### 分词器 MLBot.NLTK
> 可以直接使用jieba.net的数据
> 自带数据在App_Data目录，整个复制到应用所在目录即直接使用。无数据时，会自动创建目录及文件。
### 分词命令行工具 MLBot.NLTK.CLI
> MLBot.NLTK.CLI "很多读者指出了不朽凡人的不足，有些我认为说的是对的，有些我认为和我的想法不同。如果说不朽凡人结尾的不好，我同意。要说烂尾，我真的不同意。关于量劫带来宇宙涅化的结尾，我还是比较满意的。因为在这里有各种人性的显露，人性在生死存亡面前的脆弱。"

### 训练模型 LinyeeSeq2Seq
> seq2seq，基于开源项目[1]，进行一些优化。优点，一对一，对答N溜。缺点，一对多，对答不太行，需要变通下或用检索对答。
### 训练工具 LinyeeSeq2SeqTest
> 默认 human_cn_ws.txt robot_cn_ws.txt 一行对应一行。

# 跨平台性
* 因为本人C/C++不是很精通，所以程序控制台直接依赖dotnet core的跨平台性。另外会另起一个asp.net core 网站项目用webapi来实现跨平台。
* 有强力C/C++人员加入后再考虑自实现跨平台性。

# 设计初衷
* 用于微信公众号的自动聊天功能、与及自动处理业务的功能。尽可能的通用与广泛性支持。
* 逐步向高并发（信息爆炸）长时唠（信息锁链）多专业 高IQ高EQ高AQ发展

# MLBot 任务与规划
> 任务与规划，当然实际操作和实现时，可能会打乱期属、顺序，也可能到我上天了也完成不了。
> 有空时我会专门写个更详尽的任务规则
* 初期依赖微信
* 二期会自写一个简单的Android App
* 三期完善Android APP
* 四期尝试支持 H5、Android APP、小程序、快应用、FaceBook 
* 五期尝试支持IOS App
* 六期尝试微内核技术 裸机开发 MLBotOs ，这里需要有会汇编、C/C++且精通linux内核、虚拟机技术的人员参与。

# MLBot 维护成员招募中
> 维护成员招募中，欢迎大家一起参与进来，努力打造一个开放的开源的高智能机器人项目
* 前端H5 css 一人 已到位
* Android App 一人 招募中
* IOS APP 一人 招募中
* 其它任何您觉得你适合的细节任务都可以申请

# 授权
遵循  [MIT 授权](https://github.com/jiaguoxinzhi/MLBot/blob/master/LICENSE)

# 已知成熟Ai对话机器人
* 微软小冰
* 图灵机器人：（好像收费，自己找吧）
* [BotSharp](https://github.com/SciSharp/BotSharp)
* [思知](https://www.ownthink.com/)
* [微信对话开放平台](https://openai.weixin.qq.com/)

# 词汇解释
* IQ：Intelligence Quotient，智力商数，简称智商。学习能力、实践能力。
* EQ：Emotional Quotient，指情绪商数，简称情商。沟通能力、协作能力、情绪控制能力。
* AQ：Adversity Quotient，逆境商数，简称逆商或挫商。主动进取、承压能力。
* ?Q：自身控制能力（运动能力、毛孔控制、心跳控制、呼吸控制、图辨、声辨、嗅辨、味辨、触感、电力监控、温度监控、陀罗仪、重力感应、加速度感应）。

# 当前开发阶段 
> √完成 ×中止 △有Bug …进行中 ▲有严重Bug ☆优先开发中 ■终止开发（基本是作废）

* √尝试自建分词项目
* √尝试支持微信公众号的webHook 已初步具有自动回答的功能 支持AES的哦
* √筛选对话语料完成[2]。
* √△目前来说，先使用Seq2Seq模型[1]，但这个模型有个缺点，同一个问题如果有多种答法，他只能答一种，另外如果问题描述与原句区别比较大时，经常答非所问，可能是我用的方式不对吧。欢迎PR。
* √Seq2Seq支持训练和再训练，人类文本一个文件，机器人一个文件。两个文件需要行数一致。
* …正式测试技能添加功能，如果可以实现常见需要，将不再完善本项目功能。主要转移为“微信对话开放平台”技能开发。

# 未完内容
* …因为一些原因，此项目暂时进入维护模式，短期内不再增加功能模块。欢迎PR。
* …构思框架与将要延伸的方向
* …尝试添加一些常用RESTful api
* …Redis功能，还没有得空整理。欢迎PR。
* …后期版本计划增加ASR、NLU(意图识别能力)、KG、TTS。

# 第三方Api
> 一些功能初期都尽可能先使用第三方的接口
> 目前未得空集成，待得空了再集成，先收集整理中。。
* ASR：建议直接使用 [微信智能接口](https://developers.weixin.qq.com/doc/offiaccount/Intelligent_Interface/AI_Open_API.html)
* TTS：建议直接使用 百度语音合成接口
* NLU：尝试直接使用 [微信智能接口](https://developers.weixin.qq.com/doc/offiaccount/Intelligent_Interface/Natural_Language_Processing.html)

# 服务与支持
* 目前开通网站支持 [hotml.net](https://hotml.net/)

# 参考
1. [Seq2SeqLearn](https://github.com/mashmawy/Seq2SeqLearn)
2. chatterbot-1k 抱歉，忘了是从哪里下的。
3. 测试用的微信公众号 
> ![avatar](/mpqrcode.jpg)
