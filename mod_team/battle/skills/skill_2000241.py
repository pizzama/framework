from skills.script import Script


class skill_2000241(Script):
    def execute(self):
        #添加目标
        en = self.master

        # 添加buff
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.5,0.6,0.7,0.9,1.2]


        if (self.master.hp/self.master.maxhp) <= 0.3:
            hurt, htp = self.normal_hurt(en)
            hurt = 0
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
            # 创建技能日志
            self.create_skill_log(skldata)
            
            # 给自己加一个防御增加的buff
            buf = self.create_buff(self.master, "buf_def", {
                    "buff_id":"11003",
                    "value":en.defen*damage[level],
                })
                
            self.skill_buff(en, buf)

    def get_skill_type(self):
        return 4   
        
