from skills.script import Script


class skill_4001211(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num = len(enemys)

        # 产生伤害
        level = self.skill.level
        skill4level = self.get_my_skill_level("400124")
        skldata = self.create_skl_data()
        damage = [0,0.65,0.8,0.95,0.105,0.120]
        maxdanders=[0,20,40,60,80,100]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            # 增加特殊标记
            hurt, htp = self.normal_hurt(en)
            hurt = hurt * (1 + 0.5 * (3 - num))*damage[level]
            real_hurt = en.hurt(self, hurt)
            maxdander = maxdanders[skill4level]
            self.master.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)
