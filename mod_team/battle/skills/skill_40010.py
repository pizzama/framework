from skills.script import Script

class skill_40010(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        skldata = self.create_skl_data()
        addhurt=[0,1,0.6,0.4]
        defens=[0,0.02,0.04,0.06,0.08,0.1]
        skill4level = self.get_my_skill_level("400104")
        num = self.master.get_extraparams("leiyin")
        endefen=0
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = addhurt[num]*hurt + en.defen*defens[skill4level]*num
            endefen=endefen + en.defen*defens[skill4level]*num
            real_hurt = en.hurt(self,hurt)
            maxdander = 50
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        self.master.update_maxdander(100)
        skldata.set_eng(self.master)
        
        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)

        # 给自己添加雷印buff
        if skill4level > 0:
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id":"1401041",
                "value":endefen,
                "count":1,
                })

            self.skill_buff(self.master, buf)
        
            if num < 6:
                num = num+1
                self.master.set_extraparams("leiyin", num)

    def get_skill_type(self):
        return 0
