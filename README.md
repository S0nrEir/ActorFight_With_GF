# ActorFight_With_GF
一个基于GF框架的战斗例子，随便写写。。。持续施工
Unity Ver:2021.3.7f1
------------------------------------------------------------
引用/库<br />
Unity GameFrameWork前端框架 [GameFramework](https://github.com/EllanJiang/GameFramework)

[GameFrameworkExtension](https://github.com/FingerCaster/UGFExtensions)

[LuBan](https://github.com/focus-creative-games/luban)

[EPPlus]([https://github.com/EPPlusSoftware/EPPlus)

---

效果类TODO:

- [x] 每损失10%生命，提供自身5%暴攻击，持续x秒
- [ ] 嘲讽，强制攻击自身，持续x秒
- [ ] 每收到一次攻击提供自身1%攻击加成，持续x秒
- [x] 修改属性类型系统，修改为：角色持有基础的成长数据（原Actor_Base_Attr）及一次为依据计算出的属性(Actor_Attr)，角色属性的初始化应当以Actor_Attr为主
- [ ] 目标选择器effect
- [x] ~~修改异步的async void情况，封装一个协程~~
- [ ] Effect的前置、检查、生效、后置等逻辑封装成流程化
