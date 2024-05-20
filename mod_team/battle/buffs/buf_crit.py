from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_crit(Buff):  # 暴击变化
    def add_execute(self):
        value = self.target.crit + self.value
        self.target.set_crit(value)

    def destory(self):  # 销毁buff时恢复暴击
        value = self.target.crit - self.value
        self.target.set_crit(value)
