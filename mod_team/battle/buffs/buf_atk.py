from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_atk(Buff):  # 攻击变化
    def add_execute(self):
        value = self.target.attack + self.value
        self.target.set_atk(value)

    def destory(self):  # 销毁buff时恢复攻击力
        value = self.target.attack - self.value
        self.target.set_atk(value)
