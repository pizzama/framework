from skills.script import Script
import random

class skill_2000441(Script):
    def execute(self):

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        defens = [0,5,7,9,11,14]
        hps=[0,375,563,750,938,1125]
        rd=random.randint(1,100)
        
        if rd < 20:
            en=self.master
            
            # 给自己加一个防御增加的buff
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id":"1200441",
                "value": defens[level],
            })

            self.skill_buff(en, buf)

            # 给自己加一个生命上限的buff
            buf = self.create_buff(self.master, "buf_maxhp", {
                "buff_id":"11005",
                "value": hps[level],
            })

            self.skill_buff(en, buf)
            
            hurt = -hps[level]
            real_hurt = en.hurt(self, hurt)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            
            # 创建技能日志
            self.create_skill_log(skldata)
            
    def get_skill_type(self):
        return 4   
        

