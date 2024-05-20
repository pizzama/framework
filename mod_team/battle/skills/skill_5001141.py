from skills.script import Script


class skill_5001141(Script):
    def execute(self):

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        hurt1 = [0,0.05,0.1,0.15,0.2,0.25]
        subhurts=[0,0.01,0.02,0.03,0.04,0.05]
        hurt2 =[0,0.01,0.02,0.03,0.04,0.05]
        value = self.master.get_extraparams("zengshang")
        en = self.master
        hurt, htp = self.normal_hurt(en)
        hurt = 0
        real_hurt = en.hurt(self, hurt)
        skldata.set_eng(en)
        skldata.add_des(en)
        skldata.add_hurt(real_hurt)
        skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)


        # 增加自己攻击力
        addatk = hurt1[level]
        if addatk <= hurt2[level]:
           addatk = hurt2[level]
        else:
            addatk=hurt1[level]-(subhurts[level]*value)
            value = value + 1
            self.master.set_extraparams("zengshang", value)

        buf = self.create_buff(self.master, "buf_atk", {
            "buff_id":"11001",
            "value":self.master.attack*addatk,
            "count":1,
        })

        self.skill_buff(en, buf)
    
    def get_skill_type(self):
        return 4   