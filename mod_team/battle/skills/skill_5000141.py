from skills.script import Script


class skill_5000141(Script):
    def execute(self):

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.08,0.1,0.12,0.16,0.20]
        en = self.master
        hurt = -self.master.attack*damage[level]
        real_hurt = en.hurt(self, hurt)
        skldata.set_eng(en)
        skldata.add_des(en)
        skldata.add_hurt(real_hurt)
        skldata.add_htp(0)
        
        # 创建技能日志
        self.create_skill_log(skldata)
    
    def get_skill_type(self):
        return 4   


