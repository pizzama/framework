from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_speed(Buff):
    def init(self):
        pass

    def add_execute(self):
        value = self.target.speed + self.value
        self.target.set_speed(value)

    def destory(self):  # 销毁buff时恢复速度
        value = self.target.speed - self.value
        self.target.set_speed(value)
