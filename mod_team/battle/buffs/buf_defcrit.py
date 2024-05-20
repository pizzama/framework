from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_defcrit(Buff):  # 韧性变化
    def add_execute(self):
        value = self.target.defcrit + self.value
        self.target.set_defcrit(value)

    def destory(self):  # 销毁buff
        value = self.target.defcrit - self.value
        self.target.set_defcrit(value)
