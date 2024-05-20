from skills.script import Script


class skill_4000651(Script):
    def execute(self):
        # 筛选目标

        # 产生伤害

        skldata = self.create_skl_data()
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        hurt, htp = self.normal_hurt(self.master)
        hurt = 0
        real_hurt = self.master.hurt(self, hurt)
        skldata.set_eng(self.master)
        skldata.add_des(self.master)
        skldata.add_hurt(real_hurt)
        skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        buf = self.create_buff(self.master, "buf_def",{
            "buff_id": "11004",
            "value":self.master.defen*0.2,
            "count":3,
        })
        self.skill_buff(self.master, buf)

        buf = self.create_buff(self.master, "buf_def",{
            "buff_id": "11007",
            "value":self.master.speed*0.6,
            "count":3,
        })
        self.skill_buff(self.master, buf)

    def get_skill_type(self):
        return 4   
