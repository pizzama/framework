from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_effectshit(Buff):  # 血量上限buff
    def add_execute(self):
        value = self.target.effectshit + self.value
        self.target.set_effectshit(value)

    def destory(self):  # 销毁buff时恢复血量上限
        value = self.target.effectshit - self.value
        self.target.set_effectshit(value)
