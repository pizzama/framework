from skills.script import Script


class skill_4000311(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.25,1.5,1.75,2.0,2.25]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        buff_arr = en.has_buff_id(["13001", "13002"])
        if len(buff_arr) > 0:
           skill4level = self.get_my_skill_level("400034")
           hps=[0,-0.05,-0.1,-0.15,-0.2,-0.25]
           hp = self.master.attack*hps[skill4level]
           self.master.hurt(self,hp)
           skldata.add_des(self.master)
           skldata.add_hurt(hp)

        # 创建技能日志
        self.create_skill_log(skldata)

        selfatk=self.master.attack
        poison = [0,0.15,0.2,0.25,0.3,0.35]
        for en in enemys:
            # 给敌人加一个持续伤害的buff
            buf = self.create_buff(self.master, "buf_hp", {
                "buff_id":"13002",
                "count": 3,
                "value": poison[level]*selfatk,
            })
           
            rt = buf.is_effect(self.master, en)
            if rt:
               self.skill_buff(en, buf)

        
