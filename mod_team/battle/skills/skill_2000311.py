from skills.script import Script


class skill_2000311(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.15,1.25,1.35,1.45,1.55]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)


        # 创建技能日志
        self.create_skill_log(skldata)
