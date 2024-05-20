from skills.script import Script


class skill_4000851(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,2.5,2.5,2.5,2.5,2.5]
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

        # 创建技能日志
        self.create_skill_log(skldata)
        
        for en in enemys:
            # 降低目标防御
            endefens =[0,-0.25,-0.25,-0.25,-0.25,-0.25]
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id": "11004",
                "value":en.defen*endefens[level]
            })
            

            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)


