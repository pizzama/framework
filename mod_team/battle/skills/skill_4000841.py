from skills.script import Script


class skill_4000841(Script):
    def execute(self):
        # 筛选目标

        damages=[0,0.1,0.15,0.2,0.3,0.4]
        skldata = self.create_skl_data()
        level = self.skill.level
        en=self.master
        hurt, htp = self.normal_hurt(en)
        hurt = 0
        real_hurt = en.hurt(self, hurt)
        skldata.set_eng(en)
        skldata.add_des(en)
        skldata.add_hurt(real_hurt)
        skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        # 为自己添加反伤buff        
        buf = self.create_buff(self.master, "buf_thorns", {
            "buff_id":"13008",
            "value": en.attack*damages[level],
            "count":5,
        })

        self.skill_buff(en, buf)
    
    def get_skill_type(self):
        return 4   
                

