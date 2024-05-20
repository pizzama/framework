from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_link(Buff):  # 灵魂链接
    def init(self):
        pass

    def hurt_execute(self, skill, hurt, total):
        if hurt > 0:  # 对于伤害进处理
            for tar in self.value:  # 灵魂锁链的人受伤害
                tar.base_hurt(hurt)
            return hurt
        return 0
