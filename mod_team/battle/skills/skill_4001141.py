from skills.script import Script


class skill_4001141(Script):
    def execute(self):
        #筛选目标
        en = self.master

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.7,0.8,1,1.2,1.5]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        value = self.master.get_extraparams("cd")
        if value < 2:
            if (self.master.hp/self.master.maxhp) < 0.3 and value==0:
                hurt = -self.master.attack*damage[level]
                real_hurt = en.hurt(self, hurt)
                skldata.set_eng(en)
                skldata.add_des(en)
                skldata.add_hurt(real_hurt)
                skldata.add_htp(0)

                 # 创建技能日志
                self.create_skill_log(skldata)
            value=value+1
        else:
            value=0
        en.set_extraparams("cd", value)
    
    def get_skill_type(self):
        return 4   
                
               
             

