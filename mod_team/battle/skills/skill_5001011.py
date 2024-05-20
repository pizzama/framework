from skills.script import Script


class skill_5001011(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)
        # 产生伤害
        level = self.skill.level
        level4 = self.get_my_skill_level("500104")#查找500104技能的等级
        skldata = self.create_skl_data()
        damage = [0,1.6,2.0,2.4,2.8,3.2]
        danders= [0,-100,-120,-140,-160,-200]
        addrate=[0,0.2,0.25,0.3,0.4,0.5]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1+addrate[level4])*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self, hurt)
            en.update_maxdander(danders[level])
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)



