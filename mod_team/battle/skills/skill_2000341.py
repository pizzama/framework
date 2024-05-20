from skills.script import Script


class skill_2000341(Script):
    def execute(self):

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.05,0.1,0.15,0.2,0.25]
        en=self.master
        hurt, htp = self.normal_hurt(en)
        hurt = -en.attack*damage[level]
        real_hurt = en.hurt(self, hurt)
        skldata.set_eng(en)
        skldata.add_des(en)
        skldata.add_hurt(real_hurt)
        skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)
        
    def get_skill_type(self):
        return 4   

