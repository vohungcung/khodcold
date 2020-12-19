alter proc SP_GetTest (@TopicID int)
as

select Q.QID,Q.Title,Q.SubTitle,Q.SubTitleM,Q.TopicID,Q.QT,Q.Pos,Q.Require,Q.Content
,case when  exists(select QID from Questions where ParentID=Q.QID) then 1 else 0 end HasSub 
,dbo.FN_GetMaxLen(Q.QID) ML
from Questions Q where Q.TopicID =@TopicID
 and isnull(ParentID,0) = 0 order by Q.Pos
 go

 alter proc SP_GetSubQuestion (@ParentID int)
as

select  QID,Title,Require,QT,dbo.FN_GetMaxLen(QID) ML,Require,Content from Questions where ParentID= @ParentID
order by Pos

go

alter function FN_GetMaxLen(@QID int)
returns int 
as
begin
declare @len int,@minl int,@th int
select @len=max(len(title)),@minl=min(len(title)) from Answers where QID=@QID

select @len=isnull(@len,0)
select @minl=isnull(@minl,0)

select @th=(@len+@minl)/2
select @th=@th*7
if(@th<50)
select @th=50

return @th
end