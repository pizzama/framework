from skills.script import Script


class skill_5001241(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_random_target(1)

        # 为目标增加特殊锁魂标记
        increase=[0,0.005,0.01,0.015,0.05,0.025]
        skldata = self.create_skl_data()
        level = self.skill.level
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = 0
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
         # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            num = en.get_extraparams("suohun")
            if num <=10:
                #为敌人添加易伤buff
                buf = self.create_buff(self.master, "buf_hurt", {
                    "buff_id":"1501241",
                    "value":increase[level]*num,
                    "count":5,
                })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)
                    num = num+1
                    en.set_extraparams("suohun", num)
         
    
    def get_skill_type(self):
        return 4   
                

