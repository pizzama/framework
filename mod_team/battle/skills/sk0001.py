from skills.script import Script


class sk0001(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_scope_property(">", "atk", 2)
        if len(enemys) == 0:
            enemys = self.enemys_target_default()

        # 产生伤害
        hurts = []
        for en in enemys:
            hurt = self.master.attack - en.defen
            en.hurt(self, hurt)
            hurts.append(hurt)

        self.create_skill_log(enemys, hurts)
