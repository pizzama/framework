from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_accumulation_atk(Buff):  # 攻击变化
    def init(self):
        self.round = 0 # 经过了几次
        self.record = 0 # 累计了几次
        self.maxtotal = 10 # 最大累加次数

    def pre_execute(self): 
        self.round += 1
        if self.round >= 4:
            if self.record >= self.maxtotal: # 如果已经达到了最大值则不在处理
                return
            # 不使用记录的方式是因为战斗中会动态改变攻击力，所以只能减去攻击力
            self.target.attack - self.value * self.record
            self.record += 1
            value = self.target.attack + self.value * self.record
            self.target.set_atk(value)

