from skills.script import Script


class skill_3000451(Script):
    def execute(self):
        # 筛选目标
        enemys = self.heros_all()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,-0.7,-0.7,-0.7,-0.7,-0.7]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*hurt
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

