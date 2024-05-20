from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_def(Buff):  # 防御效果
    def add_execute(self):
        value = self.target.defen + self.value
        self.target.set_def(value)

    def destory(self):  # 销毁buff时恢复防御力
        value = self.target.defen - self.value
        self.target.set_def(value)
