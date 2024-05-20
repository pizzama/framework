from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_hurt(Buff):  # 易伤,伤害减免
    def init(self):
        pass

    def hurt_execute(self, skill, hurt, total):
        if hurt > 0:  # 对于伤害进处理
            hurt = self.value * hurt + hurt
            skill.battlelog.buff_only_record(self, self.src.id, [self.target.id], [hurt])
        return hurt
