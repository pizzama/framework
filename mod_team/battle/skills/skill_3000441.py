from skills.script import Script


class skill_3000441(Script):
    def execute(self):
        # 筛选目标
        enemys = self.heros_all()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        hurts = [0,-0.02,-0.04,-0.06,-0.8,-0.10]
        


        # 给己方所有人增加免伤
        num = self.master.get_extraparams("mianshang")
           
        if num < 3:
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
                buf = self.create_buff(self.master, "buf_hurt", {
                    "buff_id": "1300441",
                    "value":(3-num)*hurts[level],
                    "count":1,
                })

                self.skill_buff(en, buf)
            num = num+1
            self.master.set_extraparams("mianshang", num)
                
    def get_skill_type(self):
        return 4   