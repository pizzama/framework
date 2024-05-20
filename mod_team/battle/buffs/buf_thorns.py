from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_thorns(Buff):  # 反伤
    def init(self):
        self.sort = 99

    def hurt_execute(self, skill, hurt, total):
        if hurt > 0:  # 对于伤害进处理
            hurt = self.value
            skill.master.base_hurt(hurt)
            skill.battlelog.lck_record(self.src.id)
            skill.battlelog.buff_action_record(self, self.src.id, [skill.master.id], [hurt])
            skill.battlelog.rls_record(self.src.id)
            return 0
        return 0
